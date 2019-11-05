// ReSharper disable once CheckNamespace
namespace Fluent
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Interop;
    using Fluent.Automation;
    using Fluent.Automation.Peers;
    using Fluent.Internal.KnownBoxes;

    /// <summary>
    /// Represents backstage tab item
    /// </summary>
    public class BackstageTabItem : ContentControl, IKeyTipedControl, IHeaderedControl, ILogicalChildSupport, ITabItem, IRawElementProviderSimple
    {
        #region Icon

        /// <summary>
        /// Gets or sets Icon for the element
        /// </summary>
        public object Icon
        {
            get { return this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Dependency property for <see cref="Icon"/>
        /// </summary>
        public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof(BackstageTabItem), new PropertyMetadata(RibbonControl.OnIconChanged));

        #endregion

        #region KeyTip

        /// <inheritdoc />
        public string KeyTip
        {
            get { return (string)this.GetValue(KeyTipProperty); }
            set { this.SetValue(KeyTipProperty, value); }
        }

        /// <summary>
        /// Dependency property for <see cref="KeyTip"/>
        /// </summary>
        public static readonly DependencyProperty KeyTipProperty = Fluent.KeyTip.KeysProperty.AddOwner(typeof(BackstageTabItem));

        #endregion

        #region CanAddToQuickAccessToolBar

        /// <summary>
        /// Gets or sets whether button can be added to quick access toolbar
        /// </summary>
        public bool CanAddButtonToQuickAccessToolBar
        {
            get { return (bool)this.GetValue(CanAddButtonToQuickAccessToolBarProperty); }
            set { this.SetValue(CanAddButtonToQuickAccessToolBarProperty, value); }
        }

        /// <summary>Identifies the <see cref="CanAddButtonToQuickAccessToolBar"/> dependency property.</summary>
        public static readonly DependencyProperty CanAddButtonToQuickAccessToolBarProperty = DependencyProperty.Register(nameof(CanAddButtonToQuickAccessToolBar), typeof(bool), typeof(BackstageTabItem), new PropertyMetadata(false, RibbonControl.OnCanAddToQuickAccessToolBarChanged));

        #endregion

        #region IsSelected

        /// <summary>
        /// Gets or sets a value indicating whether the tab is selected
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Dependency property for <see cref="IsSelected"/>
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(typeof(BackstageTabItem),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox,
                FrameworkPropertyMetadataOptions.Journal |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.AffectsParentMeasure,
                OnIsSelectedChanged,
                CoerceIsSelectedChanged));

        #endregion

        #region IsReadOnly

        /// <summary>
        /// Gets or sets IsReadOnly for the element.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsReadOnly.  
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = RibbonProperties.IsReadOnlyProperty.AddOwner(typeof(BackstageTabItem), new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BackstageTabItem backstageTabItem)
            {
                AutomationPeerHelper.RaiseAutomationPropertyChangedEvent(backstageTabItem, AutomationElement.IsEnabledProperty, e.OldValue, e.NewValue);
            }
        }

        #endregion

        /// <summary>
        /// Gets parent tab control
        /// </summary>
        internal BackstageTabControl TabControlParent
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as BackstageTabControl;
            }
        }

        /// <summary>
        /// Gets or sets tab items text
        /// </summary>
        public object Header
        {
            get { return this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Text.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(BackstageTabItem), new PropertyMetadata());

        /// <summary>
        /// Static constructor
        /// </summary>
        static BackstageTabItem()
        {
            var type = typeof(BackstageTabItem);
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(typeof(BackstageTabItem)));
        }

        #region Overrides

        /// <inheritdoc />
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (this.IsSelected
                && this.TabControlParent != null)
            {
                this.TabControlParent.SelectedContent = newContent;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (ReferenceEquals(e.Source, this)
                || this.IsSelected == false)
            {
                this.IsSelected = true;
            }
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space
                || e.Key == Key.Enter)
            {
                this.SetCurrentValue(IsSelectedProperty, true);
            }

            base.OnKeyDown(e);
        }

        #endregion

        #region Private methods

        // Handles IsSelected changed
        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var backstageTabItem = (BackstageTabItem)d;
            var newValue = (bool)e.NewValue;

            if (newValue)
            {
                if (backstageTabItem.TabControlParent != null
                    && ReferenceEquals(backstageTabItem.TabControlParent.ItemContainerGenerator.ContainerFromItem(backstageTabItem.TabControlParent.SelectedItem), backstageTabItem) == false)
                {
                    UnselectSelectedItem(backstageTabItem.TabControlParent);

                    backstageTabItem.TabControlParent.SelectedItem = backstageTabItem.TabControlParent.ItemContainerGenerator.ItemFromContainer(backstageTabItem);
                }

                backstageTabItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, backstageTabItem));
                AutomationPeerHelper.RaiseAutomationEvent(SelectionItemPattern.ElementSelectedEvent, backstageTabItem);
            }
            else
            {
                backstageTabItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, backstageTabItem));
            }
        }

        private static object CoerceIsSelectedChanged(DependencyObject d, object baseValue)
        {
            if (d is BackstageTabItem tabItem
                && tabItem.IsReadOnly)
            {
                return BooleanBoxes.FalseBox;
            }

            if (baseValue is bool isSelected)
            {
                if (isSelected)
                {
                    return BooleanBoxes.TrueBox;
                }
                else
                {
                    return BooleanBoxes.FalseBox;
                }
            }

            return BooleanBoxes.FalseBox;
        }

        private static void UnselectSelectedItem(BackstageTabControl backstageTabControl)
        {
            if (backstageTabControl?.SelectedItem == null)
            {
                return;
            }

            if (backstageTabControl.ItemContainerGenerator.ContainerFromItem(backstageTabControl.SelectedItem) is BackstageTabItem backstageTabItem)
            {
                backstageTabItem.IsSelected = false;
            }
        }

        #endregion

        /// <summary>
        /// Handles selected event
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnSelected(RoutedEventArgs e)
        {
            this.HandleIsSelectedChanged(e);
        }

        /// <summary>
        /// Handles unselected event
        /// </summary>
        /// <param name="e">The event data.</param>
        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            this.HandleIsSelectedChanged(e);
        }

        #region Event handling

        /// <summary>
        /// Handles IsSelected changed
        /// </summary>
        /// <param name="e">The event data.</param>
        private void HandleIsSelectedChanged(RoutedEventArgs e)
        {
            this.RaiseEvent(e);
        }

        #endregion

        /// <inheritdoc />
        public KeyTipPressedResult OnKeyTipPressed()
        {
            UnselectSelectedItem(this.TabControlParent);

            this.IsSelected = true;
            this.Focus();

            return new KeyTipPressedResult(this.IsFocused, false);
        }

        /// <inheritdoc />
        public void OnKeyTipBack()
        {
        }

        #region Overrides

        /// <inheritdoc />
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new BackstageTabItemWrapperAutomationPeer(this);
        }

        #endregion

        #region ILogicalChildSupport

        /// <inheritdoc />
        void ILogicalChildSupport.AddLogicalChild(object child)
        {
            this.AddLogicalChild(child);
        }

        /// <inheritdoc />
        void ILogicalChildSupport.RemoveLogicalChild(object child)
        {
            this.RemoveLogicalChild(child);
        }

        #endregion

        #region IRawElementProviderSimple

        ProviderOptions IRawElementProviderSimple.ProviderOptions => ProviderOptions.ClientSideProvider;

        IRawElementProviderSimple IRawElementProviderSimple.HostRawElementProvider => Window.GetWindow(this) is Window window
            ? AutomationInteropProvider.HostProviderFromHandle(new WindowInteropHelper(window).Handle)
            : null;

        private BackstageTabItemAutomationPeer internalPeer = null;

        private BackstageTabItemAutomationPeer InternalPeer => this.internalPeer 
            ?? (this.internalPeer = (BackstageTabItemAutomationPeer)this.OnCreateAutomationPeer());

        object IRawElementProviderSimple.GetPatternProvider(int patternId)
        {
            if (patternId == SelectionItemPattern.Pattern.Id)
            {
                return this.OnCreateAutomationPeer();
            }

            return null;
        }

        object IRawElementProviderSimple.GetPropertyValue(int propertyId)
        {
            if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
            {
                return this.InternalPeer.GetName();
            }
            else if (propertyId == AutomationElementIdentifiers.ClassNameProperty.Id)
            {
                return this.InternalPeer.GetClassName();
            }
            else if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id)
            {
                return this.InternalPeer.GetAutomationControlType();
            }
            else if (propertyId == AutomationElementIdentifiers.IsContentElementProperty.Id)
            {
                return this.InternalPeer.IsContentElement();
            }
            else if (propertyId == AutomationElementIdentifiers.IsControlElementProperty.Id)
            {
                return this.InternalPeer.IsControlElement();
            }
            else if (propertyId == AutomationElementIdentifiers.LabeledByProperty.Id)
            {
                return this.InternalPeer.GetLabeledBy();
            }
            else if (propertyId == AutomationElementIdentifiers.IsKeyboardFocusableProperty.Id)
            {
                return this.InternalPeer.IsKeyboardFocusable();
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ITabItem

        ITabContainer ITabItem.TabControlParent => this.TabControlParent;

        #endregion
    }
}