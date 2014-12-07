using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;
using System.Windows.Threading;
using VCA_player.ViewModel;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace VCA_player.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                AuthWindow w = new AuthWindow();
                w.ShowDialog();
                InitializeComponent();

                MainViewModel.Instance.AudioPlayerVM.PropertyChanged += Instance_PropertyChanged;

                Grip.MouseDown += grip_MouseDown;
                Grip.MouseMove += Grip_MouseMove;
                Grip.MouseUp += grip_MouseUp;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (WindowStyle != WindowStyle.None)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    (DispatcherOperationCallback) delegate(object unused)
                    {
                        WindowStyle = WindowStyle.None;
                        return null;
                    }
                    , null);
            }
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (Grip.IsMouseCaptured)
            {
                this.Height = e.GetPosition(this).Y;
            }
        }

        private void grip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Grip.CaptureMouse();
            }
        }

        private void grip_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Grip.IsMouseCaptured)
            {
                Grip.ReleaseMouseCapture();
            }
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                Dispatcher.Invoke(
                    () => { TaskbarItemInfo.ProgressValue = MainViewModel.Instance.AudioPlayerVM.Position; });
            }
            if (e.PropertyName == "IsPlaying")
            {
                Dispatcher.Invoke(() =>
                {
                    TaskbarItemInfo.ProgressState = MainViewModel.Instance.AudioPlayerVM.IsPlaying
                        ? TaskbarItemProgressState.Normal
                        : TaskbarItemProgressState.Paused;
                });
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ClickCount == 2)
            {
                WindowMaximizeButton_Click(WindowMaximizeButton, new RoutedEventArgs());
                WindowMaximizeButton.IsChecked = WindowState == WindowState.Maximized;
            }
            else
            {
                DragMove();
            }
        }

        private void WindowCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void WindowHideButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            WindowState = System.Windows.WindowState.Minimized;
        }
    }
}