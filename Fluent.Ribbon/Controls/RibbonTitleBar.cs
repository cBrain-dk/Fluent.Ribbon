// ReSharper disable once CheckNamespace
namespace Fluent
{
    using System;
    using System.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Fluent.Extensions;
    using Fluent.Helpers;
    using Fluent.Internal;
    using Fluent.Internal.KnownBoxes;
    using WindowChrome = ControlzEx.Windows.Shell.WindowChrome;

    /// <summary>
    /// Represents title bar
    /// </summary>
    [StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(RibbonContextualTabGroup))]
    [TemplatePart(Name = "PART_QuickAccessToolbarHolder", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_HeaderHolder", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_ItemsContainer", Type = typeof(Panel))]
    public class RibbonTitleBar : HeaderedItemsControl
    {
        #region Fields

        // Quick access toolbar holder
        private FrameworkElement quickAccessToolbarHolder;
        // Header holder
        private FrameworkElement headerHolder;
        // Items container
        private Panel itemsContainer;
        // Quick access toolbar rect
        private Rect quickAccessToolbarRect;
        // Header rect
        private Rect headerRect;
        // Items rect
        private Rect itemsRect;
        // Width of window commands (minimize, maximize, close)
        private readonly double windowCommandWidth;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets quick access toolbar
        /// </summary>
        public FrameworkElement QuickAccessToolBar
        {
            get { return (FrameworkElement)this.GetValue(QuickAccessToolBarProperty); }
            set { this.SetValue(QuickAccessToolBarProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for QuickAccessToolBar.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty QuickAccessToolBarProperty =
            DependencyProperty.Register(nameof(QuickAccessToolBar), typeof(FrameworkElement), typeof(RibbonTitleBar), new PropertyMetadata(OnQuickAccessToolBarChanged));

        private static void OnQuickAccessToolBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var titleBar = (RibbonTitleBar)d;
            titleBar.ForceMeasureAndArrange();
        }

        /// <summary>
        /// Gets or sets header alignment
        /// </summary>
        public HorizontalAlignment HeaderAlignment
        {
            get { return (HorizontalAlignment)this.GetValue(HeaderAlignmentProperty); }
            set { this.SetValue(HeaderAlignmentProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for HeaderAlignment.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HeaderAlignmentProperty =
            DependencyProperty.Register(nameof(HeaderAlignment), typeof(HorizontalAlignment), typeof(RibbonTitleBar), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Defines whether title bar is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)this.GetValue(IsCollapsedProperty); }
            set { this.SetValue(IsCollapsedProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="IsCollapsed"/>
        /// </summary>
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(nameof(IsCollapsed), typeof(bool), typeof(RibbonTitleBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        private bool isAtLeastOneRequiredControlPresent;

        /// <summary>
        /// Using a DependencyProperty as the backing store for HideContextTabs.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HideContextTabsProperty =
            DependencyProperty.Register(nameof(HideContextTabs), typeof(bool), typeof(RibbonTitleBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        ///  Gets or sets whether context tabs are hidden.
        /// </summary>
        public bool HideContextTabs
        {
            get { return (bool)this.GetValue(HideContextTabsProperty); }
            set { this.SetValue(HideContextTabsProperty, value); }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Static constructor
        /// </summary>
        static RibbonTitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonTitleBar), new FrameworkPropertyMetadata(typeof(RibbonTitleBar)));

            HeaderProperty.OverrideMetadata(typeof(RibbonTitleBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTitleBar()
        {
            WindowChrome.SetIsHitTestVisibleInChrome(this, true);
            var windowCommands = UIHelper.FindVisualChildByName<FrameworkElement>(Window.GetWindow(this), "PART_WindowCommands");
            if (windowCommands == null || windowCommands.ActualWidth == 0)
            {
                this.windowCommandWidth = 110;
            }
            else
            {
                this.windowCommandWidth = windowCommands.ActualWidth;
            }
        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            var baseResult = base.HitTestCore(hitTestParameters);

            if (baseResult == null)
            {
                return new PointHitTestResult(this, hitTestParameters.HitPoint);
            }

            return baseResult;
        }

        /// <inheritdoc />
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            if (e.Handled
                || this.IsMouseDirectlyOver == false)
            {
                return;
            }

            WindowSteeringHelper.ShowSystemMenu(this, e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.Handled)
            {
                return;
            }

            // Contextual groups shall handle mouse events
            if (e.Source is RibbonContextualGroupsContainer
                || e.Source is RibbonContextualTabGroup)
            {
                return;
            }

            WindowSteeringHelper.HandleMouseLeftButtonDown(e, true, true);
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RibbonContextualTabGroup();
        }

        /// <inheritdoc />
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is RibbonContextualTabGroup;
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.quickAccessToolbarHolder = this.GetTemplateChild("PART_QuickAccessToolbarHolder") as FrameworkElement;
            this.headerHolder = this.GetTemplateChild("PART_HeaderHolder") as FrameworkElement;
            this.itemsContainer = this.GetTemplateChild("PART_ItemsContainer") as Panel;

            this.isAtLeastOneRequiredControlPresent = this.quickAccessToolbarHolder != null
                                     || this.headerHolder != null
                                     || this.itemsContainer != null;

            if (this.quickAccessToolbarHolder != null)
            {
                WindowChrome.SetIsHitTestVisibleInChrome(this.quickAccessToolbarHolder, true);
            }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            if (constraint.Width < 1)
            {
                return constraint;
            }

            if (this.isAtLeastOneRequiredControlPresent == false)
            {
                return base.MeasureOverride(constraint);
            }

            var resultSize = constraint;

            if (double.IsPositiveInfinity(resultSize.Width)
                || double.IsPositiveInfinity(resultSize.Height))
            {
                resultSize = base.MeasureOverride(resultSize);
            }

            if (this.QuickAccessToolBar is QuickAccessToolBar quickAccessToolBar)
            {
                this.Update(resultSize, quickAccessToolBar.PreMeasure);
            }
            else
            {
                this.Update(resultSize, this.quickAccessToolbarHolder.Measure);
            }

            this.itemsContainer.Measure(this.itemsRect.Size);
            this.headerHolder.Measure(this.headerRect.Size);
            this.quickAccessToolbarHolder.Measure(this.quickAccessToolbarRect.Size);

            var maxHeight = Math.Max(Math.Max(this.itemsRect.Height, this.headerRect.Height), this.quickAccessToolbarRect.Height);
            var width = this.itemsRect.Width + this.headerRect.Width + this.quickAccessToolbarRect.Width;

            return new Size(width, maxHeight);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (this.isAtLeastOneRequiredControlPresent == false)
            {
                return base.ArrangeOverride(arrangeBounds);
            }

            this.itemsContainer.Arrange(this.itemsRect);
            this.headerHolder.Arrange(this.headerRect);
            this.quickAccessToolbarHolder.Arrange(this.quickAccessToolbarRect);

            this.EnsureCorrectLayoutAfterArrange();

            return arrangeBounds;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sometimes the relative position only changes after the arrange phase.
        /// To compensate such sitiations we issue a second layout pass by invalidating our measure.
        /// This situation can occur if, for example, the icon of a ribbon window has it's visibility changed.
        /// </summary>
        private void EnsureCorrectLayoutAfterArrange()
        {
            var currentRelativePosition = this.GetCurrentRelativePosition();
            this.RunInDispatcherAsync(() => this.CheckPosition(currentRelativePosition, this.GetCurrentRelativePosition()));
        }

        private void CheckPosition(Point previousRelativePosition, Point currentRelativePositon)
        {
            if (previousRelativePosition != currentRelativePositon)
            {
                this.InvalidateMeasure();
            }
        }

        private Point GetCurrentRelativePosition()
        {
            var parentUIElement = this.Parent as UIElement;

            if (parentUIElement == null)
            {
                return default;
            }

            return this.TranslatePoint(default, parentUIElement);
        }

        // Update items size and positions
        private void Update(Size constraint, Action<Size> measureQuickAccess)
        {
            var visibleGroups = this.Items.OfType<RibbonContextualTabGroup>()
                            .Where(group => group.InnerVisibility == Visibility.Visible && group.Items.Count > 0)
                            .ToList();

            var canRibbonTabControlScroll = false;
            var quickAccessItemsConstraint = new Size(constraint.Width * 0.25, constraint.Height);

            // Defensively try to find out if the RibbonTabControl can scroll
            if (visibleGroups.Count > 0)
            {
                var firstVisibleItem = visibleGroups.First().FirstVisibleItem;

                canRibbonTabControlScroll = UIHelper.GetParent<RibbonTabControl>(firstVisibleItem)?.CanScroll == true;
            }

            if (this.IsCollapsed)
            {
                // Collapse QuickAccessToolbar
                this.quickAccessToolbarRect = new Rect(0, 0, 0, 0);

                // Collapse itemRect
                this.itemsRect = new Rect(0, 0, 0, 0);

                this.headerHolder.Measure(new Size(constraint.Width, constraint.Height));
                this.headerRect = new Rect(0, 0, this.headerHolder.DesiredSize.Width, constraint.Height);
            }
            else if (visibleGroups.Count == 0
                || canRibbonTabControlScroll)
            {
                // Collapse itemRect
                this.itemsRect = new Rect(0, 0, 0, 0);

                // Set quick launch toolbar and header position and size
                measureQuickAccess(quickAccessItemsConstraint);

                Size quickAccessToolbarSize = this.quickAccessToolbarHolder.DesiredSize;
                if (constraint.Width > quickAccessToolbarSize.Width + 50)
                {
                    this.quickAccessToolbarRect = new Rect(0, 0, quickAccessToolbarSize.Width, quickAccessToolbarSize.Height);
                    this.headerHolder.Measure(SizeConstants.Infinite);
                    var allTextWidth = constraint.Width - quickAccessToolbarSize.Width;

                    if (this.HeaderAlignment == HorizontalAlignment.Left)
                    {
                        this.headerRect = new Rect(quickAccessToolbarSize.Width, 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                    }
                    else if (this.HeaderAlignment == HorizontalAlignment.Center)
                    {
                        double headerSpace = 25;
                        double windowWidth = Window.GetWindow(this)?.ActualWidth ?? 0;
                        var headerStartPos = (windowWidth / 2) - (this.headerHolder.DesiredSize.Width / 2);

                        if ((quickAccessToolbarSize.Width + this.headerHolder.DesiredSize.Width + this.windowCommandWidth) < windowWidth
                            && quickAccessToolbarSize.Width + headerSpace < headerStartPos)
                        {
                            this.headerRect = new Rect(
                                x: (windowWidth / 2) - (this.headerHolder.DesiredSize.Width / 2),
                                y: 0,
                                width: this.headerHolder.DesiredSize.Width,
                                height: constraint.Height);
                        }
                        else
                        {
                            this.headerRect = new Rect(quickAccessToolbarSize.Width + headerSpace + Math.Max(0, (this.headerHolder.DesiredSize.Width / 2) - (this.headerHolder.DesiredSize.Width / 2)), 0, this.headerHolder.DesiredSize.Width, constraint.Height);
                        }
                    }
                    else if (this.HeaderAlignment == HorizontalAlignment.Right)
                    {
                        this.headerRect = new Rect(quickAccessToolbarSize.Width + Math.Max(0, allTextWidth - this.headerHolder.DesiredSize.Width), 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                    }
                    else if (this.HeaderAlignment == HorizontalAlignment.Stretch)
                    {
                        this.headerRect = new Rect(quickAccessToolbarSize.Width, 0, allTextWidth, constraint.Height);
                    }
                }
                else
                {
                    this.headerRect = new Rect(Math.Max(0, constraint.Width - 50), 0, 50, constraint.Height);
                }
            }
            else
            {
                var pointZero = default(Point);

                // get initial StartX value
                var startX = visibleGroups.First().FirstVisibleItem.TranslatePoint(pointZero, this).X;
                var endX = 0D;

                //Get minimum x point (workaround)
                foreach (var group in visibleGroups)
                {
                    var currentStartX = group.FirstVisibleItem.TranslatePoint(pointZero, this).X;

                    if (currentStartX < startX)
                    {
                        startX = currentStartX;
                    }

                    var lastItem = group.LastVisibleItem;
                    var currentEndX = lastItem.TranslatePoint(new Point(lastItem.DesiredSize.Width, 0), this).X;

                    if (currentEndX > endX)
                    {
                        endX = currentEndX;
                    }
                }

                // Ensure that startX and endX are never negative
                startX = Math.Max(0, startX);
                endX = Math.Max(0, endX);

                // Ensure that startX respect min width of QuickAccessToolBar
                startX = Math.Max(startX, this.QuickAccessToolBar?.MinWidth ?? 0);

                // Set contextual groups position and size
                this.itemsContainer.Measure(SizeConstants.Infinite);
                var itemsRectWidth = Math.Min(this.itemsContainer.DesiredSize.Width, Math.Max(0, Math.Min(endX, constraint.Width) - startX));
                this.itemsRect = new Rect(startX, 0, itemsRectWidth, constraint.Height);

                // Set quick launch toolbar position and size
                measureQuickAccess(quickAccessItemsConstraint);

                Size quickAccessToolbarSize = this.quickAccessToolbarHolder.DesiredSize;
                this.quickAccessToolbarRect = new Rect(0, 0, Math.Min(quickAccessToolbarSize.Width, startX), quickAccessToolbarSize.Height);

                if (quickAccessToolbarSize.Width > startX)
                {
                    measureQuickAccess(this.quickAccessToolbarRect.Size);
                    quickAccessToolbarSize = this.quickAccessToolbarHolder.DesiredSize;
                    this.quickAccessToolbarRect = new Rect(0, 0, quickAccessToolbarSize.Width, quickAccessToolbarSize.Height);
                }

                // Set header
                this.headerHolder.Measure(SizeConstants.Infinite);

                switch (this.HeaderAlignment)
                {
                    case HorizontalAlignment.Left:
                        {
                            if (startX - quickAccessToolbarSize.Width > 150)
                            {
                                var allTextWidth = startX - quickAccessToolbarSize.Width;
                                this.headerRect = new Rect(quickAccessToolbarSize.Width, 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                            }
                            else
                            {
                                var allTextWidth = Math.Max(0, constraint.Width - endX);
                                this.headerRect = new Rect(Math.Min(endX, constraint.Width), 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                            }
                        }

                        break;

                    case HorizontalAlignment.Center:
                        {
                            var allTextWidthRight = Math.Max(0, constraint.Width - endX);
                            var allTextWidthLeft = Math.Max(0, startX - quickAccessToolbarSize.Width);
                            var fitsRightButNotLeft = allTextWidthRight >= this.headerHolder.DesiredSize.Width && allTextWidthLeft < this.headerHolder.DesiredSize.Width;

                            double headerSpace = 25;
                            double windowWidth = Window.GetWindow(this)?.ActualWidth ?? 0;
                            var headerStartPos = (windowWidth / 2) - (this.headerHolder.DesiredSize.Width / 2);
                            var headerEndPos = (windowWidth / 2) + (this.headerHolder.DesiredSize.Width / 2);

                            if ((quickAccessToolbarSize.Width + itemsRectWidth + this.headerHolder.DesiredSize.Width + this.windowCommandWidth) < windowWidth)
                            {
                                //Place text middle if there is enough space from quickAccess and the contextual tabs are not in the middle space
                                if (quickAccessToolbarSize.Width + headerSpace < headerStartPos
                                    && endX < headerStartPos
                                    && headerEndPos < startX)
                                {
                                    this.headerRect = new Rect(
                                        x: (windowWidth / 2) - (this.headerHolder.DesiredSize.Width / 2),
                                        y: 0,
                                        width: this.headerHolder.DesiredSize.Width,
                                        height: constraint.Height);
                                }
                                else if (this.headerHolder.DesiredSize.Width + headerSpace <= allTextWidthRight)
                                {
                                    //Place text around contextual tabs, prefer placing it to the right
                                    this.headerRect = new Rect(
                                        x: endX + headerSpace,
                                        y: 0,
                                        width: this.headerHolder.DesiredSize.Width,
                                        height: constraint.Height);
                                }
                                else if (this.headerHolder.DesiredSize.Width + headerSpace <= allTextWidthLeft)
                                {
                                    //Place the text on the left side of the contextual tabs
                                    this.headerRect = new Rect(
                                        x: startX - headerSpace - this.headerHolder.DesiredSize.Width,
                                        y: 0,
                                        width: this.headerHolder.DesiredSize.Width,
                                        height: constraint.Height);
                                }
                                else
                                {
                                    //Revert back to standard fluent Center calculation
                                    if (((startX - quickAccessToolbarSize.Width < 150 || fitsRightButNotLeft) && (startX - quickAccessToolbarSize.Width > 0) && (startX - quickAccessToolbarSize.Width < constraint.Width - endX)) || (endX < constraint.Width / 2))
                                    {
                                        this.headerRect = new Rect(Math.Min(Math.Max(endX, (constraint.Width / 2) - (this.headerHolder.DesiredSize.Width / 2)), constraint.Width), 0, Math.Min(allTextWidthRight, this.headerHolder.DesiredSize.Width), constraint.Height);
                                    }
                                    else
                                    {
                                        this.headerRect = new Rect(quickAccessToolbarSize.Width + Math.Max(0, (allTextWidthLeft / 2) - (this.headerHolder.DesiredSize.Width / 2)), 0, Math.Min(allTextWidthLeft, this.headerHolder.DesiredSize.Width), constraint.Height);
                                    }
                                }
                            }
                            else
                            {
                                this.headerRect = new Rect(quickAccessToolbarSize.Width + headerSpace + Math.Max(0, (this.headerHolder.DesiredSize.Width / 2) - (this.headerHolder.DesiredSize.Width / 2)), 0, this.headerHolder.DesiredSize.Width, constraint.Height);
                            }
                        }

                        break;

                    case HorizontalAlignment.Right:
                        {
                            if (startX - quickAccessToolbarSize.Width > 150)
                            {
                                var allTextWidth = Math.Max(0, startX - quickAccessToolbarSize.Width);
                                this.headerRect = new Rect(quickAccessToolbarSize.Width + Math.Max(0, allTextWidth - this.headerHolder.DesiredSize.Width), 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                            }
                            else
                            {
                                var allTextWidth = Math.Max(0, constraint.Width - endX);
                                this.headerRect = new Rect(Math.Min(Math.Max(endX, constraint.Width - this.headerHolder.DesiredSize.Width), constraint.Width), 0, Math.Min(allTextWidth, this.headerHolder.DesiredSize.Width), constraint.Height);
                            }
                        }

                        break;

                    case HorizontalAlignment.Stretch:
                        {
                            if (startX - quickAccessToolbarSize.Width > 150)
                            {
                                var allTextWidth = startX - quickAccessToolbarSize.Width;
                                this.headerRect = new Rect(quickAccessToolbarSize.Width, 0, allTextWidth, constraint.Height);
                            }
                            else
                            {
                                var allTextWidth = Math.Max(0, constraint.Width - endX);
                                this.headerRect = new Rect(Math.Min(endX, constraint.Width), 0, allTextWidth, constraint.Height);
                            }
                        }

                        break;
                }
            }

            this.headerRect.Width = this.headerRect.Width + 2;
        }

        #endregion
    }
}