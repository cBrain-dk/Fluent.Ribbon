﻿namespace Fluent
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.IO.IsolatedStorage;
  using System.Linq;
  using System.Text;
  using System.Windows;
  using System.Windows.Markup;
  using System.Windows.Threading;
  using Fluent.Extensions;

  /// <summary>
  /// Handles loading and saving the state of a <see cref="Ribbon"/> from/to a <see cref="MemoryStream"/>, for temporary storage, and from/to <see cref="IsolatedStorage"/>, for persistent storage.
  /// </summary>
  public class StringRibbonStateStorage : IRibbonStateStorage
  {
    /// <summary>
    /// Getter
    /// </summary>
    public static string GetSettings(DependencyObject obj)
    {
      return (string)obj.GetValue(SettingsProperty);
    }

    /// <summary>
    /// Setter
    /// </summary>
    public static void SetSettings(DependencyObject obj, string value)
    {
      obj.SetValue(SettingsProperty, value);
    }


    /// <summary>
    /// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    /// </summary>
    public static readonly DependencyProperty SettingsProperty =
        DependencyProperty.RegisterAttached("Settings", typeof(string), typeof(StringRibbonStateStorage), new FrameworkPropertyMetadata(null, OnSettingsChanged) { BindsTwoWayByDefault = true });

    /// <summary>
    /// Settings changed
    /// </summary>
    public static void OnSettingsChanged(DependencyObject targetObject, DependencyPropertyChangedEventArgs e)
    {
      Ribbon self = (Ribbon)targetObject;

      self.RibbonStateStorage.Load();
    }


    private readonly Ribbon ribbon;

        // Name of the isolated storage file
        private string isolatedStorageFileName;
        private readonly Stream memoryStream;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="ribbon">The <see cref="Ribbon"/> of which the state should be stored.</param>
        public StringRibbonStateStorage(Ribbon ribbon)
        {
            this.ribbon = ribbon;
            this.memoryStream = new MemoryStream();
        }


    /// <summary>
    /// Destructor for this instance.
    /// </summary>
    ~StringRibbonStateStorage()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets wether state is currently saving.
        /// </summary>
        public bool IsSaving { get; private set; }
        /// <summary>
        /// Gets wether this object already got disposed.
        /// </summary>
        protected bool Disposed { get; private set; }

        /// <summary>
        /// Gets wether state is currently loading.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Gets or sets whether state is loaded.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        ///     Gets name of the isolated storage file
        /// </summary>
        protected string IsolatedStorageFileName
        {
            get
            {
                if (this.isolatedStorageFileName != null)
                {
                    return this.isolatedStorageFileName;
                }

                var stringForHash = "";
                var window = Window.GetWindow(this.ribbon);

                if (window != null)
                {
                    stringForHash += "." + window.GetType().FullName;

                    if (string.IsNullOrEmpty(window.Name) == false
                        && window.Name.Trim().Length > 0)
                    {
                        stringForHash += "." + window.Name;
                    }
                }

                if (string.IsNullOrEmpty(this.ribbon.Name) == false
                    && this.ribbon.Name.Trim().Length > 0)
                {
                    stringForHash += "." + this.ribbon.Name;
                }

                this.isolatedStorageFileName = "Fluent.Ribbon.State.2.0." + stringForHash.GetHashCode().ToString("X");
                return this.isolatedStorageFileName;
            }
        }

        /// <summary>
        /// Save current state to a temporary storage.
        /// </summary>
        public virtual void SaveTemporary()
        {
            this.memoryStream.Position = 0;
            this.Save(this.memoryStream);
        }

        /// <summary>
        /// Save current state to a persistent storage.
        /// </summary>
        public virtual void Save()
        {
            // Check whether automatic save is valid now
            if (this.ribbon.AutomaticStateManagement == false)
            {
                Debug.WriteLine("State not saved to isolated storage. Because automatic state management is disabled.");
                return;
            }

            if (this.IsLoaded == false)
            {
                Debug.WriteLine("State not saved to isolated storage. Because state was not loaded before.");
                return;
            }

            try
            {
              this.IsSaving = true;
              MemoryStream stream = new MemoryStream();
              this.Save(stream);
              stream.Position = 0;
              using (var streamReader = new StreamReader(stream))
              {
                 SetSettings(ribbon, streamReader.ReadToEnd());
              }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while trying to save Ribbon state. Error: {ex}");
            }
            finally
            {
              this.IsSaving = false;
            }
        }

        /// <summary>
        /// Saves state to <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">Stream</param>
        protected virtual void Save(Stream stream)
        {
            // Don't save or load state in design mode
            if (DesignerProperties.GetIsInDesignMode(this.ribbon))
            {
                return;
            }

            var builder = this.CreateStateData();

            var writer = new StreamWriter(stream);
            writer.Write(builder.ToString());

            writer.Flush();
        }

        /// <summary>
        /// Create the serialized state data which should be saved later.
        /// </summary>
        /// <returns><see cref="StringBuilder"/> which contains the serialized state data.</returns>
        protected virtual StringBuilder CreateStateData()
        {
            var builder = new StringBuilder();

            var isMinimizedSaveState = this.ribbon.IsMinimized;

            // Save Ribbon State
            builder.Append(isMinimizedSaveState.ToString(CultureInfo.InvariantCulture));
            builder.Append(',');
            builder.Append(this.ribbon.ShowQuickAccessToolBarAboveRibbon.ToString(CultureInfo.InvariantCulture));
            builder.Append('|');

            // Save QAT items
            var paths = new Dictionary<FrameworkElement, string>();
            this.ribbon.TraverseLogicalTree(this.ribbon, "", paths);

            // Foreach items and see whether path is found for the item
            foreach (var element in this.ribbon.QuickAccessToolBar.Items)
            {
              string path;
              FrameworkElement control = this.ribbon.GetQuickAccessElement(element) as FrameworkElement;

              if (control != null
                  && paths.TryGetValue(control, out path))
              {
                builder.Append(control.Name);
                builder.Append(';');
              }
              else
              {
                // Item is not found in logical tree, output to debug console
                var controlName = (control != null && string.IsNullOrEmpty(control.Name) == false)
                    ? string.Format(CultureInfo.InvariantCulture, " (name of the control is {0})", control.Name)
                    : string.Empty;

                //Debug.WriteLine("Control " + element.Key.GetType().Name + " is not found in logical tree during QAT saving" + controlName);
              }
            }
            return builder;
        }

        /// <summary>
        /// Load state from a temporary storage.
        /// </summary>
        public virtual void LoadTemporary()
        {
            this.memoryStream.Position = 0;
            this.Load(this.memoryStream);
        }

        /// <summary>
        /// Loads the State from Isolated Storage (in user store for domain)
        /// </summary>
        /// <remarks>
        /// Sets <see cref="IsLoaded" /> after it's finished to prevent a race condition with saving the state to the MemoryStream.
        /// </remarks>
        public virtual void Load()
        {
            if (this.IsSaving)
              return;
            if (this.ribbon.QuickAccessToolBar == null)
              return;
            // Don't save or load state in design mode
            if (DesignerProperties.GetIsInDesignMode(this.ribbon))
            {
                Debug.WriteLine("State not loaded from isolated storage. Because we are in design mode.");
                this.IsLoaded = true;
                return;
            }

            if (this.ribbon.AutomaticStateManagement == false)
            {
                this.IsLoaded = true;
                Debug.WriteLine("State not loaded from isolated storage. Because automatic state management is disabled.");
                return;
            }

            try
            {
              if (!string.IsNullOrEmpty(GetSettings(ribbon)))
              {
                byte[] byteArray = Encoding.UTF8.GetBytes(GetSettings(ribbon));
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                  this.Load(stream);
                  // Copy loaded state to MemoryStream for temporary storage.
                  // Temporary storage is used for style changes etc. so we can apply the current state again.
                  stream.Position = 0;
                  this.memoryStream.Position = 0;
                  stream.CopyTo(this.memoryStream);
                }

              }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error while trying to load Ribbon state. Error: {ex}");
            }

            this.IsLoaded = true;
        }

        /// <summary>
        /// Loads state from <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the state from.</param>
        protected virtual void Load(Stream stream)
        {
            this.IsLoading = true;

            try
            {
                this.LoadStateCore(stream);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Loads state from <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the state from.</param>
        protected virtual void LoadStateCore(Stream stream)
        {
            var reader = new StreamReader(stream);
            var data = reader.ReadToEnd();
            this.LoadState(data);
        }

        /// <summary>
        /// Loads state from <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The <see cref="string"/> to load the state from.</param>
        protected virtual void LoadState(string data)
        {
            var splitted = data.Split('|');

            if (splitted.Length != 2)
            {
                return;
            }

            // Load Ribbon State
            var ribbonProperties = splitted[0].Split(',');

            var isMinimized = bool.Parse(ribbonProperties[0]);

            this.ribbon.IsMinimized = isMinimized;

            this.ribbon.ShowQuickAccessToolBarAboveRibbon = bool.Parse(ribbonProperties[1]);

            this.LoadQuickAccessItems(splitted[1]);
        }

        /// <summary>
        /// Loads quick access items from <paramref name="quickAccessItemsData"/>.
        /// </summary>
        /// <param name="quickAccessItemsData">Serialized data for generating quick access items.</param>
        protected virtual void LoadQuickAccessItems(string quickAccessItemsData)
        {
            // Load items
            var items = quickAccessItemsData.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            this.ribbon.ClearQuickAccessToolBar();

            foreach (var item in items)
            {
                var quickAccessItem = this.CreateQuickAccessItem(item);

                if (quickAccessItem != null)
                {
                    this.ribbon.AddToQuickAccessToolBar(quickAccessItem);
                }
            }

            // Since application might not fully loaded we have to delay the refresh
            if (this.ribbon.QuickAccessToolBar != null)
            {
                this.ribbon.RunInDispatcherAsync(this.ribbon.QuickAccessToolBar.Refresh, DispatcherPriority.Background);
            }

            // Sync QAT menu items
            foreach (var menuItem in this.ribbon.QuickAccessItems)
            {
              if (menuItem.Target == null && menuItem.TargetName != null)
              {
                menuItem.Target = ribbon.FindElement(menuItem.TargetName, ribbon);

              }
              menuItem.IsChecked = this.ribbon.IsInQuickAccessToolBar(menuItem.Target);
            }
        }

        /// <summary>
        /// Creates a quick access item (<see cref="UIElement"/>) from the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Serialized data for one quick access item.</param>
        /// <returns>The created quick access item or <c>null</c> of the creation failed.</returns>
        protected virtual UIElement CreateQuickAccessItem(string data)
        {
            var result = this.ribbon.FindElement(data, ribbon);

            if (result == null
                || QuickAccessItemsProvider.IsSupported(result) == false)
            {
                // Item is invalid
                Debug.WriteLine($"Error while QAT items loading. Could not add \"{data}\" to QAT.");
                return null;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the given file exists in the given storage
        /// </summary>
        protected static bool IsolatedStorageFileExists(IsolatedStorageFile storage, string fileName)
        {
            var files = storage.GetFileNames(fileName);
            return files.Length != 0;
        }

        /// <summary>
        /// Get this <see cref="IsolatedStorageFile"/> which should be used to store the current state.
        /// </summary>
        /// <returns><see cref="IsolatedStorageFile.GetUserStoreForDomain"/> or <see cref="IsolatedStorageFile.GetUserStoreForAssembly"/> if <see cref="IsolatedStorageFile.GetUserStoreForDomain"/> threw an exception.</returns>
        protected static IsolatedStorageFile GetIsolatedStorageFile()
        {
            try
            {
                return IsolatedStorageFile.GetUserStoreForDomain();
            }
            catch
            {
                return IsolatedStorageFile.GetUserStoreForAssembly();
            }
        }

        /// <summary>
        /// Resets saved state.
        /// </summary>
        public virtual void Reset()
        {
            var storage = GetIsolatedStorageFile();

            foreach (var filename in storage.GetFileNames("*Fluent.Ribbon.State*"))
            {
                storage.DeleteFile(filename);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Defines wether managed resources should also be freed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.Disposed)
            {
                return;
            }

            if (disposing)
            {
                this.memoryStream.Dispose();
            }

            this.Disposed = true;
        }


  }
}