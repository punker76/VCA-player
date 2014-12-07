using Microsoft.Practices.Prism.Events;
using Rina.Infastructure.Models;
using Rina.Modules.AudioPlayer.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows;
using System;
using Rina.Infastructure.Interfaces;
using System.Windows.Input;
using System.Diagnostics;
using Rina.StyleResources.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Rina.Modules.AudioPlayer.Views
{
    [Export]
    public sealed partial class AudioListView : UserControl, IPartImportsSatisfiedNotification
    {
        //private DragHelper dragHelper;

        public AudioListView()
        {
            InitializeComponent();
            this.dragScope = Application.Current.MainWindow.Content as FrameworkElement;
            //this.dragHelper = new DragHelper(uiAudioList, new ListBoxDragDropDataProvider(uiAudioList), Application.Current.MainWindow.Content as UIElement);
        }

        [Import]
        public AudioListViewModel ViewModel
        {
            get { return DataContext as AudioListViewModel; }
            set { DataContext = value; }
        }

        [Import]
        private IEventAggregator eventAggregator { get; set; }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            this.eventAggregator.GetEvent<AudioSelectedEvent>()
                .Subscribe(item => uiAudioList.ScrollIntoView(item));
        }

        private ListBoxItem draggedItem = null;
        private DragAdorner draggedItemAdorner = null;
        private Point dragPosition = new Point();
        private FrameworkElement dragScope;// Application.Current.MainWindow.Content as FrameworkElement;
        private Boolean isDragging = false;

        private Boolean IsLyrics(ListBox source, MouseButtonEventArgs e)
        {
            UIElement element = source.InputHitTest(e.GetPosition(source)) as UIElement;

            while (element != null && element != source)
            {
                if (element is AudioItem.AudioItemLyricsView) return true;

                element = VisualTreeHelper.GetParent(element) as UIElement;
            }

            return false;
        }

        private void uiAudioList_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            this.draggedItem = null;

            if (IsLyrics(uiAudioList, e)) return;

            if (item != null)
            {
                this.draggedItem = item;
                dragPosition = e.GetPosition(this.draggedItem);
            }
        }

        private AdornerLayer layer = null;

        private void uiAudioList_MouseMove(object sender, MouseEventArgs e)
        {
            if (ShouldStartDrag(e))
            {
                this.isDragging = true;

                var item = this.draggedItem.DataContext as IAudioListItemViewModel;
                Debug.Assert(item != null);
                if (item.ShowLyrics)
                {
                    item.SwitchLyricsStateCommand.Execute(null);
                }
                var capturePoint = e.GetPosition(this.draggedItem);
                this.draggedItemAdorner = new DragAdorner(this.dragScope, this.draggedItem, true, capturePoint, 0.7);

                layer = AdornerLayer.GetAdornerLayer(this.dragScope);
                layer.Add(this.draggedItemAdorner);
                this.draggedItem.Visibility = System.Windows.Visibility.Collapsed;

                DragDrop.AddPreviewDragOverHandler(this.dragScope, DragScopeDragOver);
                DragDrop.AddGiveFeedbackHandler(this.dragScope, DragScopeGiveFeedback);
                try
                {
                    DragDrop.DoDragDrop(this.draggedItem, item, DragDropEffects.Move);
                    this.draggedItem.CaptureMouse();
                }
                finally
                {
                    DragDrop.RemovePreviewDragOverHandler(this.dragScope, DragScopeDragOver);
                    DragDrop.RemoveGiveFeedbackHandler(this.dragScope, DragScopeGiveFeedback);
                    this.draggedItem.Visibility = System.Windows.Visibility.Visible;

                    if (layer != null && this.draggedItemAdorner != null) layer.Remove(this.draggedItemAdorner);
                    this.draggedItemAdorner = null;
                    this.draggedItem = null;

                    this.isDragging = false;
                }
            }
        }

        private void DragScopeGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Mouse.SetCursor(Cursors.Hand);
            }
            else
                e.UseDefaultCursors = true;

            e.Handled = true;
        }

        private void DragScopeDragOver(object sender, DragEventArgs e)
        {
            if (this.draggedItemAdorner != null)
            {
                Point p = e.GetPosition(this.layer);
                this.draggedItemAdorner.UpdatePosition(p);
            }
        }

        private bool ShouldStartDrag(MouseEventArgs e)
        {
            if (this.draggedItem == null || this.isDragging)
                return false;

            var curPos = e.GetPosition(this.draggedItem);
            System.Diagnostics.Debug.WriteLine("Vertical: {0}; Horizontal: {1}",
                curPos.Y - dragPosition.Y,
                curPos.X - dragPosition.X);
            return e.LeftButton == MouseButtonState.Pressed
                   && (Math.Abs(curPos.Y - dragPosition.Y) > System.Windows.SystemParameters.MinimumVerticalDragDistance
                       || Math.Abs(curPos.X - dragPosition.X) > System.Windows.SystemParameters.MinimumHorizontalDragDistance);
        }

        private void uiAudioList_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var droppedData = e.Data.GetData(typeof (AudioListItemViewModel)) as AudioListItemViewModel;
            var target = ((ListBoxItem) (sender)).DataContext as IAudioListItemViewModel;

            ViewModel.Audio.MoveItem(droppedData, target);
        }
    }
}
