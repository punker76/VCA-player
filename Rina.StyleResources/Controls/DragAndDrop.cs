using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Shapes;
using System.Xml;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Rina.StyleResources.Controls
{/*
Based on code originally published by Jaime Rodriguez:
http://blogs.msdn.com/b/jaimer/archive/2007/07/12/drag-drop-in-wpf-explained-end-to-end.aspx
*/


    /*internal static class Extensions
    {
        private const int GWL_EXSTYLE = -20;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT pt);

        public static POINT? GetMouseCursorPosition()
        {
            POINT result;
            if (!GetCursorPos(out result)) return null;
            return result;
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int nValue);

        public static void SetNativeExtendedWindowStyle(this Window window, int windowStyle, bool enable)
        {
            if (window == null) throw new ArgumentNullException("window");

            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            if (hWnd == IntPtr.Zero) return;

            int style = GetWindowLong(hWnd, GWL_EXSTYLE);
            if (enable)
            {
                style |= windowStyle;
            }
            else
            {
                style &= ~windowStyle;
            }

            SetWindowLong(hWnd, GWL_EXSTYLE, style);
        }


        public static T FindVisualParent<T>(this DependencyObject element, DependencyObject stopAt = null) where T : DependencyObject
        {
            if (element == null) throw new ArgumentNullException("element");

            DependencyObject parent = element;
            var result = parent as T;

            while (result == null && parent != null && parent != stopAt)
            {
                parent = VisualTreeHelper.GetParent(parent);
                result = parent as T;
            }

            return result;
        }
    }

    [Flags]
    public enum DragDropProviderActions
    {
        None = 0,
        Data = 0x01,
        Visual = 0x02,
        Feedback = 0x04,
        ContinueDrag = 0x08,
        Clone = 0x10,
        MultiFormatData = 0x20,
        Unparent = 0x8000,
    }

    public interface IDataDropObjectProvider
    {
        DragDropProviderActions SupportedActions { get; }
        object GetData();
        void AppendData(ref IDataObject data, MouseEventArgs e);
        UIElement GetVisual(MouseEventArgs e);
        void GiveFeedback(GiveFeedbackEventArgs args);
        void ContinueDrag(QueryContinueDragEventArgs args);
        bool UnParent();
    }

    internal sealed class DragDataWrapper
    {
        public object Data;
        public bool AllowChildrenRemove;
        public IDataDropObjectProvider Shim;
    }

    public interface IDataDropTarget
    {
        bool CanDrop(DragEventArgs args, object rawData);
        void DataDropped(DragEventArgs args, object rawData);
    }

    public interface IDropTargetHilighter
    {
        object AddHilight(UIElement target);
        void RemoveHilight(UIElement target, object previousState);
    }
    */
    public sealed class DragAdorner : Adorner
    {
        private readonly UIElement child;
        private readonly double xCenter;
        private readonly double yCenter;

        private double leftOffset;
        private double topOffset;
        private Point capture;

        public DragAdorner(UIElement owner, UIElement child, bool useVisualBrush, Point capture, double opacity)
            : base(owner)
        {
            if (!useVisualBrush)
            {
                this.child = child;
            }
            else
            {
                var size = GetRealSize(child);
                this.xCenter = size.Width / 2;
                this.yCenter = size.Height / 2;
                
                this.child = new Rectangle
                {
                    RadiusX = 3,
                    RadiusY = 3,
                    Width = size.Width,
                    Height = size.Height,
                    Fill = new ImageBrush()
                    {
                        ImageSource = CreateBitmapFromVisual(child),
                        Opacity = opacity,
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top,
                        Stretch = Stretch.None,
                    }
                };
            }

            this.capture = capture;
        }

        private RenderTargetBitmap CreateBitmapFromVisual(Visual target)
        {
            Debug.Assert(target != null);

            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

            RenderTargetBitmap rtb = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(target);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            return rtb;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        private static Size GetRealSize(UIElement child)
        {
            return child == null ? Size.Empty : child.RenderSize;
        }

        public void UpdatePosition(Point point)
        {
            this.leftOffset = point.X - this.capture.X;
            this.topOffset = point.Y - this.capture.Y;
            //Debug.WriteLine("Pos: {0} : {1}", this.leftOffset, this.topOffset);
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            var adorner = Parent as AdornerLayer;
            if (adorner != null) adorner.Update(AdornedElement);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (0 != index) throw new ArgumentOutOfRangeException("index");
            return child;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            child.Measure(availableSize);
            return child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            child.Arrange(new Rect(child.DesiredSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(new TranslateTransform(leftOffset, topOffset));

            var baseTransform = base.GetDesiredTransform(transform);
            if (baseTransform != null) result.Children.Add(baseTransform);

            return result;
        }
    }
    /*
    internal sealed class DragDropWindow : Window
    {
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;

        private DragDropWindow()
        {
            InitializeComponent();
        }

        public static DragDropWindow Create(UIElement dragElement)
        {
            var result = new DragDropWindow();
            result.SetContent(dragElement);
            result.Show();
            return result;
        }

        private void InitializeComponent()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            AllowDrop = false;
            Background = null;
            IsHitTestVisible = false;
            SizeToContent = SizeToContent.WidthAndHeight;
            Topmost = true;
            ShowInTaskbar = false;
        }

        private void InitializeWindow()
        {
            this.SetNativeExtendedWindowStyle(WS_EX_LAYERED | WS_EX_TRANSPARENT, true);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            InitializeWindow();
        }

        public void SetContent(UIElement dragElement)
        {
            var element = dragElement as FrameworkElement;
            if (element != null)
            {
                Content = new Rectangle
                {
                    Width = element.ActualWidth,
                    Height = element.ActualHeight,
                    Fill = new VisualBrush(dragElement)
                };
            }
        }

        public void UpdatePosition()
        {
            var position = Extensions.GetMouseCursorPosition();
            if (position != null)
            {
                var value = position.GetValueOrDefault();
                Left = value.X;
                Top = value.Y;
            }
        }
    }

    public sealed class DropTargetVisualStateHilighter : IDropTargetHilighter
    {
        public const string HighlightVisualState = "DragOver";
        public const string NoHighlightVisualState = "NoDrag";

        public object AddHilight(UIElement target)
        {
            var el = target as FrameworkElement;
            if (el != null) VisualStateManager.GoToState(el, HighlightVisualState, true);
            return null;
        }

        public void RemoveHilight(UIElement target, object previousState)
        {
            var el = target as FrameworkElement;
            if (el != null) VisualStateManager.GoToState(el, NoHighlightVisualState, true);
        }
    }

    public class DragHelper : DependencyObject
    {
        private static readonly string DragDataWrapperKey = typeof(DragDataWrapper).ToString();
        private static readonly Point EmptyStartPoint = new Point(double.NaN, double.NaN);

        private readonly IDataDropObjectProvider _callback;
        private readonly UIElement _dragSource;
        private readonly UIElement _dragScope;
        private DragDropEffects _allowedEffects = DragDropEffects.Copy | DragDropEffects.Move;
        private double _opacity = 0.7;
        private bool _isDragging;
        private Point _startPoint = EmptyStartPoint;
        private AdornerLayer _layer;
        private DragAdorner _adorner;
        private DragDropWindow _dragdropWindow;
        private bool _mouseLeftScope;

        public DragHelper(UIElement source, IDataDropObjectProvider callback = null, UIElement dragScope = null)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (callback == null) callback = BuildDefaultProvider(source);

            _dragSource = source;
            _callback = callback;
            _dragScope = dragScope;

            source.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
            source.PreviewMouseMove += DragSource_PreviewMouseMove;
        }

        public UIElement DragSource
        {
            get { return _dragSource; }
        }

        public UIElement DragScope
        {
            get { return _dragScope; }
        }

        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        public bool IsDragging
        {
            get { return _isDragging; }
            protected set { _isDragging = value; }
        }

        public DragDropEffects AllowedEffects
        {
            get { return _allowedEffects; }
            set { _allowedEffects = value; }
        }

        protected bool AllowsLink
        {
            get { return (DragDropEffects.Link & _allowedEffects) == DragDropEffects.Link; }
        }

        protected bool AllowsMove
        {
            get { return (DragDropEffects.Move & _allowedEffects) == DragDropEffects.Move; }
        }

        protected bool AllowsCopy
        {
            get { return (DragDropEffects.Copy & _allowedEffects) == DragDropEffects.Copy; }
        }

        private static IDataDropObjectProvider BuildDefaultProvider(UIElement source)
        {
            var callback = source as IDataDropObjectProvider;
            if (callback != null) return callback;

            var list = source as Selector;
            if (list != null) return new ListBoxDragDropDataProvider(list);

            return null;
        }

        private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(DragScope);
        }

        private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging && _startPoint != EmptyStartPoint && e.LeftButton == MouseButtonState.Pressed && !IsWithinScrollBar(e.OriginalSource, e.Source))
            {
                Point position = e.GetPosition(DragScope);
                if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                   Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);
                }
            }
        }

        private void DragScope_DragLeave(object sender, DragEventArgs args)
        {
            if (ReferenceEquals(_dragSource, args.OriginalSource))
            {
                _mouseLeftScope = true;
            }
        }

        private void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (!_isDragging)
            {
                e.Action = DragAction.Cancel;
                e.Handled = true;
            }
            else if (_dragScope == null)
            {
                if (_dragdropWindow != null)
                {
                    _dragdropWindow.UpdatePosition();
                }
            }
            else
            {
                if (_adorner != null)
                {
                    Point p = Mouse.GetPosition(_dragScope);
                    _adorner.UpdatePosition(p);
                }
                if (_mouseLeftScope)
                {
                    e.Action = DragAction.Cancel;
                    e.Handled = true;
                }
            }
        }

        private void DragScope_DragOver(object sender, DragEventArgs e)
        {
            if (_adorner != null)
            {
                Point p = e.GetPosition(_dragScope);
                _adorner.UpdatePosition(p);
            }
        }

        protected virtual bool IsWithinScrollBar(object originalSource, object source)
        {
            var control = originalSource as DependencyObject;
            return control != null && control.FindVisualParent<ScrollBar>(source as DependencyObject) != null;
        }

        protected virtual DragDropEffects GetDragDropEffects()
        {
            var effects = DragDropEffects.None;
            bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (ctrl && shift && AllowsLink)
            {
                effects |= DragDropEffects.Link;
            }
            else if (ctrl && AllowsCopy)
            {
                effects |= DragDropEffects.Copy;
            }
            else
            {
                if (AllowsMove) effects |= DragDropEffects.Move;
                if (AllowsCopy) effects |= DragDropEffects.Copy;
            }

            return effects;
        }

        protected virtual void StartDrag(MouseEventArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");

            IDataObject data;
            UIElement dragElement;

            if (_callback == null)
            {
                dragElement = args.OriginalSource as UIElement;
                data = (dragElement == null) ? null : new DataObject(typeof(UIElement).ToString(), dragElement);
            }
            else
            {
                var wrapper = new DragDataWrapper
                {
                    Shim = _callback,
                    AllowChildrenRemove = (DragDropProviderActions.Unparent & _callback.SupportedActions) == DragDropProviderActions.Unparent,
                };

                data = new DataObject(DragDataWrapperKey, wrapper);

                if ((DragDropProviderActions.MultiFormatData & _callback.SupportedActions) == DragDropProviderActions.MultiFormatData)
                {
                    _callback.AppendData(ref data, args);
                }
                if ((DragDropProviderActions.Data & _callback.SupportedActions) == DragDropProviderActions.Data)
                {
                    wrapper.Data = _callback.GetData();
                }
                if ((DragDropProviderActions.Visual & _callback.SupportedActions) == DragDropProviderActions.Visual)
                {
                    dragElement = _callback.GetVisual(args);
                }
                else
                {
                    dragElement = args.OriginalSource as UIElement;
                }
            }

            if (data != null && dragElement != null && _dragSource != null && !ReferenceEquals(dragElement, _dragSource))
            {
                DragDropEffects effects = GetDragDropEffects();
                if (_dragScope != null)
                {
                    // In-window drag:
                    _layer = AdornerLayer.GetAdornerLayer(_dragScope);
                    _adorner = new DragAdorner(_dragScope, dragElement, true, _opacity);
                    _layer.Add(_adorner);

                    bool previousAllowDrop = _dragScope.AllowDrop;
                    _dragScope.AllowDrop = true;

                    _isDragging = true;
                    _mouseLeftScope = false;

                    DragDrop.AddPreviewDragOverHandler(_dragScope, DragScope_DragOver);
                    DragDrop.AddPreviewDragLeaveHandler(_dragScope, DragScope_DragLeave);
                    DragDrop.AddPreviewQueryContinueDragHandler(_dragSource, DragSource_QueryContinueDrag);

                    try
                    {
                        DragDropEffects resultEffects = DragDrop.DoDragDrop(_dragSource, data, effects);
                        DragFinished(resultEffects);
                    }
                    finally
                    {
                        DragDrop.RemovePreviewDragOverHandler(_dragScope, DragScope_DragOver);
                        DragDrop.RemovePreviewDragLeaveHandler(_dragScope, DragScope_DragLeave);
                        DragDrop.RemovePreviewQueryContinueDragHandler(_dragSource, DragSource_QueryContinueDrag);

                        _dragScope.AllowDrop = previousAllowDrop;
                        _startPoint = EmptyStartPoint;
                        _isDragging = false;

                        if (_layer != null) _layer.Remove(_adorner);
                        _adorner = null;
                        _layer = null;
                    }
                }
                else
                {
                    // Cross-window / process drag:

                    _isDragging = true;
                    _dragdropWindow = DragDropWindow.Create(dragElement);

                    DragDrop.AddPreviewQueryContinueDragHandler(_dragSource, DragSource_QueryContinueDrag);

                    try
                    {
                        DragDropEffects resultEffects = DragDrop.DoDragDrop(_dragSource, data, effects);
                        DragFinished(resultEffects);
                    }
                    finally
                    {
                        DragDrop.RemovePreviewQueryContinueDragHandler(_dragSource, DragSource_QueryContinueDrag);

                        _dragdropWindow.Close();
                        _dragdropWindow = null;

                        _startPoint = EmptyStartPoint;
                        _isDragging = false;
                    }
                }
            }
        }

        protected virtual void DragFinished(DragDropEffects resultEffects)
        {
            Mouse.Capture(null);
        }
    }

    public class DropHelper
    {
        private static readonly string UIElementKey = typeof(UIElement).ToString();
        private static readonly string DragDataWrapperKey = typeof(DragDataWrapper).ToString();
        private static readonly string[] DefaultDataTypes = new[] { DragDataWrapperKey, UIElementKey, DataFormats.Text };

        private readonly UIElement _dropTarget;
        private readonly IDataDropTarget _dataDropTarget;
        private DragDropEffects _allowedEffects = DragDropEffects.Copy | DragDropEffects.Move;
        private string[] _allowedDataTypes;

        private readonly IDropTargetHilighter _defaultHilighter = new DropTargetVisualStateHilighter();
        private readonly IDropTargetHilighter _hilighter;
        private object _hilighterState;

        public DropHelper(UIElement dropTarget, IDataDropTarget dataDropTarget = null, IDropTargetHilighter targetHilighter = null)
        {
            if (dropTarget == null) throw new ArgumentNullException("dropTarget");
            if (dataDropTarget == null) dataDropTarget = BuildDefaultTarget(dropTarget);

            _dropTarget = dropTarget;
            _dataDropTarget = dataDropTarget;
            _hilighter = targetHilighter;

            _dropTarget.AllowDrop = true;
            _dropTarget.DragEnter += DropTarget_DragEnter;
            _dropTarget.DragOver += DropTarget_DragOver;
            _dropTarget.Drop += DropTarget_Drop;
            _dropTarget.DragLeave += DropTarget_DragLeave;
        }

        protected UIElement DropTarget
        {
            get { return _dropTarget; }
        }

        protected IDataDropTarget DataDropTarget
        {
            get { return _dataDropTarget; }
        }

        protected IDropTargetHilighter TargetHilighter
        {
            get { return _hilighter ?? _defaultHilighter; }
        }

        public DragDropEffects AllowedEffects
        {
            get { return _allowedEffects; }
            set { _allowedEffects = value; }
        }

        public string[] AllowedDataTypes
        {
            get { return _allowedDataTypes ?? DefaultDataTypes; }
            set { _allowedDataTypes = value; }
        }

        private void DropTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (CanDrop(e))
            {
                AddTargetHilight();
            }
        }

        private void DropTarget_DragLeave(object sender, DragEventArgs e)
        {
            RemoveTargetHilight();
        }

        private void DropTarget_DragOver(object sender, DragEventArgs e)
        {
            if (CanDrop(e))
            {
                if (e.Effects == DragDropEffects.None)
                {
                    e.Effects = AllowedEffects;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void DropTarget_Drop(object sender, DragEventArgs e)
        {
            Drop(e);
        }

        protected virtual void AddTargetHilight()
        {
            _hilighterState = TargetHilighter.AddHilight(DropTarget);
        }

        protected virtual void RemoveTargetHilight()
        {
            try
            {
                TargetHilighter.RemoveHilight(DropTarget, _hilighterState);
            }
            finally
            {
                var state = _hilighterState as IDisposable;
                if (state != null) state.Dispose();
                _hilighterState = null;
            }
        }

        protected virtual bool CanDrop(DragEventArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Data == null) throw new ArgumentException("No data.", "args");

            bool result = false;

            IDataObject data = args.Data;
            string[] types = data.GetFormats();
            string[] allowedTypes = AllowedDataTypes;
            if (types != null && types.Length != 0 && allowedTypes != null && allowedTypes.Length != 0)
            {
                if (allowedTypes.Length == 1 && allowedTypes[0] == "*")
                {
                    result = true;
                }
                else if (types.Intersect(allowedTypes, StringComparer.OrdinalIgnoreCase).Any())
                {
                    result = true;
                }
            }
            if (result && _dataDropTarget != null)
            {
                if (data.GetDataPresent(DragDataWrapperKey))
                {
                    var wrapper = data.GetData(DragDataWrapperKey) as DragDataWrapper;
                    if (wrapper != null && wrapper.Shim != null)
                    {
                        if ((DragDropProviderActions.Data & wrapper.Shim.SupportedActions) == DragDropProviderActions.Data)
                        {
                            result = _dataDropTarget.CanDrop(args, wrapper.Data);
                        }
                    }
                }
            }

            return result;
        }

        protected virtual void Drop(DragEventArgs args)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Data == null) throw new ArgumentException("No data.", "args");

            IDataObject data = args.Data;

            bool isDataOperation = false;
            DragDataWrapper wrapper = null;
            if (data.GetDataPresent(DragDataWrapperKey))
            {
                wrapper = data.GetData(DragDataWrapperKey) as DragDataWrapper;
                if (wrapper != null && wrapper.Shim != null)
                {
                    isDataOperation = (DragDropProviderActions.Data & wrapper.Shim.SupportedActions) == DragDropProviderActions.Data;
                }
            }

            if (isDataOperation)
            {
                if (_dataDropTarget != null)
                {
                    _dataDropTarget.DataDropped(args, wrapper.Data);
                }
            }
            else
            {
                if (data.GetDataPresent(UIElementKey))
                {
                    bool unparented = false;
                    var element = (UIElement)data.GetData(UIElementKey);
                    if (args.AllowedEffects == DragDropEffects.Move)
                    {
                        unparented = Unparent(wrapper, element);
                    }
                    if (unparented)
                    {
                        Panel panel;
                        ContentControl contentControl;
                        ItemsControl itemsControl;

                        if ((panel = _dropTarget as Panel) != null)
                        {
                            panel.Children.Add(element);
                        }
                        else if ((contentControl = _dropTarget as ContentControl) != null)
                        {
                            contentControl.Content = element;
                        }
                        else if ((itemsControl = _dropTarget as ItemsControl) != null)
                        {
                            if (itemsControl.ItemsSource == null)
                            {
                                itemsControl.Items.Insert(0, element);
                            }
                        }
                    }
                }
                else if (data.GetDataPresent(DataFormats.Text))
                {
                    var value = (string)data.GetData(DataFormats.Text);

                    Panel panel;
                    ContentControl contentControl;
                    ItemsControl itemsControl;

                    if ((panel = _dropTarget as Panel) != null)
                    {
                        panel.Children.Add(new TextBlock
                        {
                            Text = value,
                            FontSize = 16,
                            TextWrapping = TextWrapping.Wrap,
                            MaxWidth = panel.ActualWidth
                        });
                    }
                    else if ((contentControl = _dropTarget as ContentControl) != null)
                    {
                        contentControl.Content = value;
                    }
                    else if ((itemsControl = _dropTarget as ItemsControl) != null)
                    {
                        if (itemsControl.ItemsSource == null)
                        {
                            itemsControl.Items.Insert(0, value);
                        }
                    }
                }
            }

            RemoveTargetHilight();
        }

        private static bool Unparent(DragDataWrapper wrapper, UIElement element)
        {
            bool success = false;
            if (wrapper != null && wrapper.Shim != null && wrapper.AllowChildrenRemove)
            {
                success = wrapper.Shim.UnParent();
            }
            if (!success) // BRUTE FORCE 
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null && frameworkElement.Parent != null)
                {
                    Panel parentPanel;
                    ContentControl parentContent;
                    ItemsControl parentItems;

                    if ((parentPanel = frameworkElement.Parent as Panel) != null)
                    {
                        parentPanel.Children.Remove(element);
                        success = true;
                    }
                    else if ((parentContent = frameworkElement.Parent as ContentControl) != null)
                    {
                        parentContent.Content = null;
                        success = true;
                    }
                    else if ((parentItems = frameworkElement.Parent as ItemsControl) != null)
                    {
                        if (parentItems.ItemsSource != null)
                        {
                            int index = parentItems.ItemContainerGenerator.IndexFromContainer(element);
                            if (index != -1)
                            {
                                parentItems.Items.RemoveAt(index);
                                success = true;
                            }
                        }
                    }
                }
            }

            return success;
        }

        private static IDataDropTarget BuildDefaultTarget(UIElement dropTarget)
        {
            var target = dropTarget as IDataDropTarget;
            if (target != null) return target;

            var list = dropTarget as ItemsControl;
            if (list != null) return new ListBoxDataDropTarget(list);

            var contentControl = dropTarget as ContentControl;
            if (contentControl != null) return new ContentControlDropTarget(contentControl);

            return null;
        }
    }

    public sealed class ContentControlDropTarget : IDataDropTarget
    {
        private readonly ContentControl _control;

        public ContentControlDropTarget(ContentControl control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        public bool CanDrop(DragEventArgs args, object rawData)
        {
            return true;
        }

        public void DataDropped(DragEventArgs args, object rawData)
        {
            _control.Content = rawData;
        }
    }

    public class ListBoxDataDropTarget : IDataDropTarget
    {
        private readonly ItemsControl _control;

        public ListBoxDataDropTarget(ItemsControl control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        public bool CanDrop(DragEventArgs args, object rawData)
        {
            var list = _control.ItemsSource as IList ?? _control.Items;
            return list != null && !list.Contains(rawData);
        }

        public void DataDropped(DragEventArgs args, object rawData)
        {
            var list = _control.ItemsSource as IList ?? _control.Items;

            if (list == null || !AddItem(list, rawData))
            {
                if (args != null)
                {
                    args.Effects = DragDropEffects.None;
                }
            }
        }

        protected virtual bool AddItem(IList items, object itemToAdd)
        {
            if (items == null || itemToAdd == null) return false;

            bool result = !items.Contains(itemToAdd);
            if (result) items.Add(itemToAdd);
            return result;
        }
    }

    public class ListBoxDragDropDataProvider : IDataDropObjectProvider
    {
        private readonly Selector _control;

        public ListBoxDragDropDataProvider(Selector control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
        }

        public DragDropProviderActions SupportedActions
        {
            get { return DragDropProviderActions.Data | DragDropProviderActions.MultiFormatData | DragDropProviderActions.Visual | DragDropProviderActions.Unparent; }
        }

        public object GetData()
        {
            return _control.SelectedItem;
        }

        public void AppendData(ref IDataObject data, MouseEventArgs e)
        {
            if (data == null) throw new ArgumentNullException("data");

            object value = _control.SelectedItem;
            if (value != null)
            {
                data.SetData(value.GetType().ToString(), value);

                var element = value as XmlElement;
                if (element != null)
                {
                    data.SetData(DataFormats.Text, element.OuterXml);
                }
                else if (!(value is string))
                {
                    data.SetData(DataFormats.Text, value.ToString());
                }
            }
        }

        public UIElement GetVisual(MouseEventArgs e)
        {
            object value = _control.SelectedItem;
            return value == null ? null : _control.ItemContainerGenerator.ContainerFromItem(value) as UIElement;
        }

        protected virtual void GiveFeedback(GiveFeedbackEventArgs args)
        {
            throw new NotSupportedException();
        }

        void IDataDropObjectProvider.GiveFeedback(GiveFeedbackEventArgs args)
        {
            GiveFeedback(args);
        }

        protected virtual void ContinueDrag(QueryContinueDragEventArgs args)
        {
            throw new NotSupportedException();
        }

        void IDataDropObjectProvider.ContinueDrag(QueryContinueDragEventArgs args)
        {
            ContinueDrag(args);
        }

        protected virtual bool UnParent()
        {
            throw new NotSupportedException();
        }

        bool IDataDropObjectProvider.UnParent()
        {
            return UnParent();
        }
    }

    public interface ICommandExecutor
    {
        bool? CanExecute(ICommand command, object parameter);
        void Execute(ICommand command, object parameter);
    }

    public class CommandDataDropTarget : IDataDropTarget
    {
        private readonly ICommand _dropCommand;
        private readonly object _dropCommandParameter;

        public CommandDataDropTarget(ICommand dropCommand, object dropCommandParameter = null)
        {
            if (dropCommand == null) throw new ArgumentNullException("dropCommand");

            _dropCommand = dropCommand;
            _dropCommandParameter = dropCommandParameter;
        }

        public ICommand DropCommand
        {
            get { return _dropCommand; }
        }

        public object DropCommandParameter
        {
            get { return _dropCommandParameter; }
        }

        public bool CanDrop(DragEventArgs args, object rawData)
        {
            var executor = rawData as ICommandExecutor;
            return executor != null && executor.CanExecute(_dropCommand, _dropCommandParameter).GetValueOrDefault();
        }

        public void DataDropped(DragEventArgs args, object rawData)
        {
            var executor = rawData as ICommandExecutor;
            if (executor != null)
            {
                Action<ICommand, object> method = executor.Execute;
                Dispatcher.CurrentDispatcher.BeginInvoke(method, DispatcherPriority.DataBind, _dropCommand, _dropCommandParameter);
            }
        }
    }*/
}
