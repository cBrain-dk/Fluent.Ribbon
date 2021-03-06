﻿// ReSharper disable once CheckNamespace
namespace Fluent
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Fluent.Extensibility;

    /// <summary>
    /// Attached Properties for the Fluent Ribbon library
    /// </summary>
    public static class RibbonProperties
    {
        #region Internal class

        internal class CommandCanExecuteChanged
        {
            private IRibbonControl RibbonControl { get; set; }

            private ICommand Command { get; set; }

            internal CommandCanExecuteChanged(IRibbonControl ribbonControl, ICommand command)
            {
                this.RibbonControl = ribbonControl;
                this.Command = command;
            }

            internal void RegisterCommand()
            {
                this.SetIsReadOnlyFromCommand(this.Command);
                this.Command.CanExecuteChanged += this.CanExecuteChanged;
            }

            internal void UnRegisterCommand()
            {
                this.RibbonControl.IsReadOnly = true;
                this.Command.CanExecuteChanged -= this.CanExecuteChanged;
            }

            private void CanExecuteChanged(object sender, EventArgs e)
            {
                if (sender is ICommand command)
                {
                    this.SetIsReadOnlyFromCommand(command);
                }
            }

            private void SetIsReadOnlyFromCommand(ICommand command)
            {
                if (command is RoutedCommand routedCommand)
                {
                    this.RibbonControl.IsReadOnly = routedCommand.CanExecute(null, this.RibbonControl as UIElement) == false;
                }
                else
                {
                    this.RibbonControl.IsReadOnly = command.CanExecute(null) == false;
                }
            }
        }

        #endregion

        #region CommandCanExectue Property

        /// <summary>
        /// Using a DependencyProperty as the backing store for TitleBarHeight.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CommandCanExectueProperty =
            DependencyProperty.RegisterAttached("CommandCanExectue", typeof(CommandCanExecuteChanged), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(null));

        internal static void SetCommandCanExectue(UIElement element, CommandCanExecuteChanged value)
        {
            element.SetValue(CommandCanExectueProperty, value);
        }

        internal static CommandCanExecuteChanged GetCommandCanExectue(UIElement element)
        {
            return element == null ? null : (CommandCanExecuteChanged)element.GetValue(CommandCanExectueProperty);
        }

        #endregion

        #region TitleBarHeight Property

        /// <summary>
        /// Using a DependencyProperty as the backing store for TitleBarHeight.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.RegisterAttached("TitleBarHeight", typeof(double), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(35D, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Sets TitleBarHeight for element
        /// </summary>
        public static void SetTitleBarHeight(UIElement element, double value)
        {
            element.SetValue(TitleBarHeightProperty, value);
        }

        /// <summary>
        /// Gets TitleBarHeight for element
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Ribbon))]
        [AttachedPropertyBrowsableForType(typeof(RibbonTitleBar))]
        [AttachedPropertyBrowsableForType(typeof(RibbonWindow))]
        public static double GetTitleBarHeight(UIElement element)
        {
            return (double)element.GetValue(TitleBarHeightProperty);
        }

        #endregion

        #region Size Property

        /// <summary>
        /// Using a DependencyProperty as the backing store for Size.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(RibbonControlSize), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(RibbonControlSize.Large,
                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                              FrameworkPropertyMetadataOptions.AffectsMeasure |
                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                              FrameworkPropertyMetadataOptions.AffectsParentArrange |
                                              FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                                              OnSizeChanged));

        /// <summary>
        /// Sets <see cref="SizeProperty"/> for <paramref name="element"/>.
        /// </summary>
        public static void SetSize(DependencyObject element, RibbonControlSize value)
        {
            element.SetValue(SizeProperty, value);
        }

        /// <summary>
        /// Gets <see cref="SizeProperty"/> for <paramref name="element"/>.
        /// </summary>
        //[AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        public static RibbonControlSize GetSize(DependencyObject element)
        {
            return (RibbonControlSize)element.GetValue(SizeProperty);
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sink = d as IRibbonSizeChangedSink;

            sink?.OnSizePropertyChanged((RibbonControlSize)e.OldValue, (RibbonControlSize)e.NewValue);
        }

        #endregion

        #region SizeDefinition Property

        /// <summary>
        /// Using a DependencyProperty as the backing store for SizeDefinition.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SizeDefinitionProperty =
            DependencyProperty.RegisterAttached("SizeDefinition", typeof(RibbonControlSizeDefinition), typeof(RibbonProperties),
                new FrameworkPropertyMetadata(new RibbonControlSizeDefinition(RibbonControlSize.Large, RibbonControlSize.Middle, RibbonControlSize.Small),
                                              FrameworkPropertyMetadataOptions.AffectsArrange |
                                              FrameworkPropertyMetadataOptions.AffectsMeasure |
                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                              FrameworkPropertyMetadataOptions.AffectsParentArrange |
                                              FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                                              OnSizeDefinitionChanged));

        /// <summary>
        /// Sets <see cref="SizeDefinitionProperty"/> for <paramref name="element"/>.
        /// </summary>
        public static void SetSizeDefinition(DependencyObject element, RibbonControlSizeDefinition value)
        {
            element.SetValue(SizeDefinitionProperty, value);
        }

        /// <summary>
        /// Gets <see cref="SizeDefinitionProperty"/> for <paramref name="element"/>.
        /// </summary>
        //[AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        public static RibbonControlSizeDefinition GetSizeDefinition(DependencyObject element)
        {
            return (RibbonControlSizeDefinition)element.GetValue(SizeDefinitionProperty);
        }

        // Handles RibbonSizeDefinitionProperty changes
        internal static void OnSizeDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Find parent group box
            var groupBox = FindParentRibbonGroupBox(d);
            var element = (UIElement)d;

            SetAppropriateSize(element, groupBox?.State ?? RibbonGroupBoxState.Large);
        }

        // Finds parent group box
        internal static RibbonGroupBox FindParentRibbonGroupBox(DependencyObject element)
        {
            var currentElement = element;
            RibbonGroupBox groupBox;

            while ((groupBox = currentElement as RibbonGroupBox) == null)
            {
                currentElement = VisualTreeHelper.GetParent(currentElement)
                    ?? LogicalTreeHelper.GetParent(currentElement);

                if (currentElement == null)
                {
                    break;
                }
            }

            return groupBox;
        }

        /// <summary>
        /// Sets appropriate size of the control according to the
        /// given group box state and control's size definition
        /// </summary>
        /// <param name="element">UI Element</param>
        /// <param name="state">Group box state</param>
        public static void SetAppropriateSize(DependencyObject element, RibbonGroupBoxState state)
        {
            SetSize(element, GetSizeDefinition(element).GetSize(state));
        }

        #endregion

        #region IsReadOnly

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsReadOnly.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(RibbonProperties), new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = d as FrameworkElement;
            if (frameworkElement != null)
            {
                if ((bool)e.NewValue == true)
                {
                    VisualStateManager.GoToState(frameworkElement, "Disabled", true);
                }
                else
                {
                    VisualStateManager.GoToState(frameworkElement, "Normal", true);
                }
            }
        }

        /// <summary>
        /// Sets IsReadOnly for element
        /// </summary>
        public static void SetIsReadOnly(DependencyObject element, bool value)
        {
            element.SetValue(IsReadOnlyProperty, value);
        }

        /// <summary>
        /// Gets IsReadOnly for element
        /// </summary>
        public static bool GetIsReadOnly(DependencyObject element)
        {
            return (bool)element.GetValue(IsReadOnlyProperty);
        }

        #endregion

        #region MouseOverBackgroundProperty

        /// <summary>
        /// <see cref="DependencyProperty"/> for specifying MouseOverBackground.
        /// </summary>
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.RegisterAttached("MouseOverBackground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets <see cref="MouseOverBackgroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        public static void SetMouseOverBackground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverBackgroundProperty, value);
        }

        /// <summary>
        /// Gets <see cref="MouseOverBackgroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        //[AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        public static Brush GetMouseOverBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverBackgroundProperty);
        }

        #endregion

        #region MouseOverForegroundProperty

        /// <summary>
        /// <see cref="DependencyProperty"/> for specifying MouseOverForeground.
        /// </summary>
        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.RegisterAttached("MouseOverForeground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets <see cref="MouseOverForegroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        public static void SetMouseOverForeground(DependencyObject element, Brush value)
        {
            element.SetValue(MouseOverForegroundProperty, value);
        }

        /// <summary>
        /// Gets <see cref="MouseOverForegroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        //[AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        public static Brush GetMouseOverForeground(DependencyObject element)
        {
            return (Brush)element.GetValue(MouseOverForegroundProperty);
        }

        #endregion

        #region IsSelectedBackgroundProperty

        /// <summary>
        /// <see cref="DependencyProperty"/> for specifying IsSelectedBackground.
        /// </summary>
        public static readonly DependencyProperty IsSelectedBackgroundProperty = DependencyProperty.RegisterAttached("IsSelectedBackground", typeof(Brush), typeof(RibbonProperties), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Sets <see cref="IsSelectedBackgroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        public static void SetIsSelectedBackground(DependencyObject element, Brush value)
        {
            element.SetValue(IsSelectedBackgroundProperty, value);
        }

        /// <summary>
        /// Gets <see cref="IsSelectedBackgroundProperty"/> for <paramref name="element"/>.
        /// </summary>
        //[AttachedPropertyBrowsableForType(typeof(IRibbonControl))]
        public static Brush GetIsSelectedBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(IsSelectedBackgroundProperty);
        }

        #endregion

        #region IsBackstageItem

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsBackstageItem.  
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsBackstageItemProperty = DependencyProperty.RegisterAttached("IsBackstageItem", typeof(bool), typeof(RibbonProperties), new PropertyMetadata(false));

        /// <summary>
        /// Sets IsBackstageItem for element
        /// </summary>
        public static void SetIsBackstageItem(DependencyObject element, bool value)
        {
            element.SetValue(IsBackstageItemProperty, value);
        }

        /// <summary>
        /// Gets IsBackstageItem for element
        /// </summary>
        public static bool GetIsBackstageItem(DependencyObject element)
        {
            return (bool)element.GetValue(IsBackstageItemProperty);
        }

        #endregion

        #region Command
        internal static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandCanExecuteChanged old = GetCommandCanExectue(d as UIElement);
            if (old != null)
            {
                old.UnRegisterCommand();
            }

            if (e.NewValue is ICommand newValue && d is IRibbonControl ribbonControl)
            {
                var temp = new CommandCanExecuteChanged(ribbonControl, newValue);
                SetCommandCanExectue((UIElement)ribbonControl, temp);
                temp.RegisterCommand();
            }
        }

        #endregion
    }
}