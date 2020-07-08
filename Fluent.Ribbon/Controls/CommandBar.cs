namespace Fluent
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Fluent.Internal.KnownBoxes;
    using WindowChrome = ControlzEx.Windows.Shell.WindowChrome;

    /// <summary>
    /// Specifies the <see cref="CommandBarDock"/> position of a child element that is inside a <see cref="CommandBar"/>.
    /// </summary>
    public enum CommandBarDock
    {
        /// <summary>
        /// A child element that is positioned on the left side of the <see cref="CommandBar"/>.
        /// </summary>
        Left = 0,

        /// <summary>
        /// A child element that is positioned on the right side of the <see cref="CommandBar"/>.
        /// </summary>
        Right = 1
    }

    /// <summary>
    /// Helper class for <see cref="CommandBarDock"/>.
    /// </summary>
    public static class CommandBarDockExtensions
    {
        /// <summary>
        /// Simple helper for converting from <see cref="CommandBarDock"/> to <see cref="Dock" />.
        /// </summary>
        public static Dock ConvertToDock(this CommandBarDock commandBarDock)
        {
            return commandBarDock switch
            {
                CommandBarDock.Right => Dock.Right,
                CommandBarDock.Left => Dock.Left,
                _ => Dock.Left,
            };
        }
    }

    /// <summary>
    /// Simpler version of a Ribbon used in windows where full complexity of <see cref="RibbonGroupBox" /> and/or <see cref="RibbonTabItem" /> are not needed.
    /// Supports content in the sizes Small and Middle (Large will be scaled down to medium)
    /// </summary>
    public class CommandBar : ItemsControl, IKeyTipServiceHost
    {
        #region Constants

        /// <summary>
        /// Minimal width of commandbar parent window
        /// </summary>
        public const double MinimalVisibleWidth = 300;

        /// <summary>
        /// Minimal height of commandbar parent window
        /// </summary>
        public const double MinimalVisibleHeight = 250;

        #endregion

        #region Fields

        private ObservableCollection<Key> keyTipKeys;

        // Handles F10, Alt and so on
        private readonly Fluent.KeyTipService keyTipService;

        private List<CommandBarItem> ControlsLeft { get; } = new List<CommandBarItem>();

        private List<CommandBarItem> ControlsRight { get; } = new List<CommandBarItem>();

        #endregion

        #region Properties

        #region CommandBarDock

        /// <summary>
        /// Attached property to for specifying relative placement of button on <see cref="CommandBar"/>.
        /// </summary>
        public static readonly DependencyProperty CommandBarDockProperty = DependencyProperty.RegisterAttached("CommandBarDock", typeof(CommandBarDock), typeof(CommandBar));

        /// <summary>Helper for getting <see cref="CommandBarDockProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="CommandBarDockProperty"/> from.</param>
        /// <returns>CommandBarDock property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static CommandBarDock GetCommandBarDock(DependencyObject element)
        {
            return (CommandBarDock)element.GetValue(CommandBarDockProperty);
        }

        /// <summary>Helper for setting <see cref="CommandBarDockProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="CommandBarDockProperty"/> on.</param>
        /// <param name="value">CommandBarDock property value.</param>
        public static void SetCommandBarDock(DependencyObject element, CommandBarDock value)
        {
            element.SetValue(CommandBarDockProperty, value);
        }

        #endregion

        #region TitelBar

        /// <summary>
        /// Gets ribbon titlebar
        /// </summary>
        public RibbonTitleBar TitleBar
        {
            get { return (RibbonTitleBar)this.GetValue(TitleBarProperty); }
            set { this.SetValue(TitleBarProperty, value); }
        }

        /// <summary>Identifies the <see cref="TitleBar"/> dependency property.</summary>
        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(nameof(TitleBar), typeof(RibbonTitleBar), typeof(CommandBar));

        #endregion

        #region ContentGapHeight

        /// <summary>
        /// Gets or sets the height of the gap between the CommandBar and the regular window content
        /// </summary>
        public double ContentGapHeight
        {
            get { return (double)this.GetValue(ContentGapHeightProperty); }
            set { this.SetValue(ContentGapHeightProperty, value); }
        }

        /// <summary>Identifies the <see cref="ContentGapHeight"/> dependency property.</summary>
        public static readonly DependencyProperty ContentGapHeightProperty =
            DependencyProperty.Register(nameof(ContentGapHeight), typeof(double), typeof(CommandBar), new PropertyMetadata(RibbonTabControl.DefaultContentGapHeight));

        #endregion

        #region KeyTips

        /// <summary>
        /// Checks if any keytips are visible.
        /// </summary>
        public bool AreAnyKeyTipsVisible => this.keyTipService?.AreAnyKeyTipsVisible == true;

        /// <summary>Identifies the <see cref="IsKeyTipHandlingEnabled"/> dependency property.</summary>
        public static readonly DependencyProperty IsKeyTipHandlingEnabledProperty = DependencyProperty.Register(nameof(IsKeyTipHandlingEnabled), typeof(bool), typeof(CommandBar), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsKeyTipHandlingEnabledChanged));

        /// <summary>
        /// Defines whether handling of key tips is enabled or not.
        /// </summary>
        public bool IsKeyTipHandlingEnabled
        {
            get { return (bool)this.GetValue(IsKeyTipHandlingEnabledProperty); }
            set { this.SetValue(IsKeyTipHandlingEnabledProperty, value); }
        }

        private static void OnIsKeyTipHandlingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbon = (CommandBar)d;

            if ((bool)e.NewValue)
            {
                ribbon.keyTipService?.Attach();
            }
            else
            {
                ribbon.keyTipService?.Detach();
            }
        }

        /// <summary>
        /// Defines the keys that are used to activate the key tips.
        /// </summary>
        public ObservableCollection<Key> KeyTipKeys
        {
            get
            {
                if (this.keyTipKeys == null)
                {
                    this.keyTipKeys = new ObservableCollection<Key>();
                    this.keyTipKeys.CollectionChanged += this.HandleKeyTipKeys_CollectionChanged;
                }

                return this.keyTipKeys;
            }
        }

        private void HandleKeyTipKeys_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.keyTipService.KeyTipKeys.Clear();

            foreach (var keyTipKey in this.KeyTipKeys)
            {
                this.keyTipService.KeyTipKeys.Add(keyTipKey);
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandBar()
        {
            this.VerticalAlignment = VerticalAlignment.Top;
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.Contained);

            WindowChrome.SetIsHitTestVisibleInChrome(this, true);

            this.keyTipService = new Fluent.KeyTipService(this);

            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private void SetItemProperties(System.Collections.IList items)
        {
            foreach (var item in items)
            {
                if (item is UIElement element
                    && item is IRibbonControl ribbonControl
                    && this.ItemContainerGenerator.ContainerFromItem(item) is UIElement container)
                {
                    CommandBarDock commandBarDock = GetCommandBarDock(element);
                    DockPanel.SetDock(container, commandBarDock.ConvertToDock());

                    if (commandBarDock == CommandBarDock.Left)
                    {
                        this.ControlsLeft.Add(new CommandBarItem(ribbonControl));
                    }
                    else
                    {
                        this.ControlsRight.Add(new CommandBarItem(ribbonControl));
                    }

                    if (ribbonControl.Size == RibbonControlSize.Large)
                    {
                        ribbonControl.Size = RibbonControlSize.Middle;
                    }
                }
            }
        }

        #endregion

        #region Event handling

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.keyTipService.Attach();

            this.SetItemProperties(this.Items);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.keyTipService.Detach();
        }

        #endregion

        #region Overrides

        private Size LastConstraint { get; set; }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.IsLoaded == false || constraint.Width == this.LastConstraint.Width)
            {
                return base.MeasureOverride(constraint);
            }

            double TryCollapseControls(List<CommandBarItem> items, double wantedWidth)
            {
                foreach (CommandBarItem item in items)
                {
                    if (item.RibbonControl.Size == RibbonControlSize.Small)
                    {
                        continue;
                    }

                    double initialWidth = item.Control.DesiredSize.Width;

                    item.RibbonControl.Size = RibbonControlSize.Small;
                    item.Control.Measure(new Size(40, constraint.Height));

                    double changedWidth = initialWidth - item.Control.DesiredSize.Width;
                    if (changedWidth > 1)
                    {
                        wantedWidth -= changedWidth;

                        if (wantedWidth < 1)
                        {
                            return wantedWidth;
                        }
                    }
                    else
                    {
                        item.RibbonControl.Size = RibbonControlSize.Middle;
                        return 0;
                    }
                }

                return wantedWidth;
            }

            double TryExpandControls(List<CommandBarItem> items, double availableWidth)
            {
                foreach (CommandBarItem item in items)
                {
                    if (item.InitialSize == RibbonControlSize.Small || item.RibbonControl.Size == RibbonControlSize.Middle)
                    {
                        continue;
                    }

                    double initialWidth = item.Control.DesiredSize.Width;

                    item.RibbonControl.Size = RibbonControlSize.Middle;
                    item.Control.Measure(new Size(double.PositiveInfinity, constraint.Height));

                    double changedWidth = item.Control.DesiredSize.Width - initialWidth;
                    if (changedWidth > 1 && changedWidth <= availableWidth)
                    {
                        availableWidth -= changedWidth;
                    }
                    else
                    {
                        item.RibbonControl.Size = RibbonControlSize.Small;
                        return 0;
                    }
                }

                return availableWidth;
            }

            bool isExpandingWindow = this.LastConstraint.Width < constraint.Width;
            if (isExpandingWindow)
            {
                double availableWidth = constraint.Width - this.DesiredSize.Width;
                if (availableWidth > 0)
                {
                    availableWidth = TryExpandControls(this.ControlsLeft, availableWidth);

                    if (availableWidth > 0)
                    {
                        var reversedRight = this.ControlsRight.ToList();
                        reversedRight.Reverse();
                        TryExpandControls(reversedRight, availableWidth);
                    }
                }
            }
            else
            {
                double wantedWidth = this.DesiredSize.Width - constraint.Width;
                if (wantedWidth > 0)
                {
                    wantedWidth = TryCollapseControls(this.ControlsRight, wantedWidth);

                    if (wantedWidth > 0)
                    {
                        var reversedLeft = this.ControlsLeft.ToList();
                        reversedLeft.Reverse();
                        TryCollapseControls(reversedLeft, wantedWidth);
                    }
                }
            }

            this.LastConstraint = constraint;

            return base.MeasureOverride(constraint);
        }

        #endregion

        #region IKeyTipServiceHost

        bool IKeyTipServiceHost.IsEnabled => this.IsEnabled;

        bool IKeyTipServiceHost.IsKeyboardFocusWithin => this.IsKeyboardFocusWithin;

        bool IKeyTipServiceHost.IsCollapsed => false;

        bool IKeyTipServiceHost.IsKeyTipHandlingEnabled => this.IsKeyTipHandlingEnabled;

        bool IKeyTipServiceHost.CanSetFocusFromKeyTipService(FrameworkElement keyTipsTarget)
            => true;

        void IKeyTipServiceHost.SetFocusFromKeyTipService()
            => this.Focus();

        FrameworkElement IKeyTipServiceHost.GetKeyTipServiceTarget()
            => this;

        Control IKeyTipServiceHost.AsControl()
            => this;

        #endregion

        private class CommandBarItem
        {
            public IRibbonControl RibbonControl { get; }

            public RibbonControlSize InitialSize { get; }

            public Control Control { get; }

            public CommandBarItem(IRibbonControl item)
            {
                this.RibbonControl = item;
                this.InitialSize = item.Size;
                this.Control = (Control)this.RibbonControl;
            }
        }
    }
}
