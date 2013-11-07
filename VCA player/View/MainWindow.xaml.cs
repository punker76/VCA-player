using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCA_player.Model;
using System.Media;
using System.Web;
using System.Net;
using VCA_player.ViewModel;
using System.Runtime.InteropServices;

namespace VCA_player.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        private const int SW_RESTORE = 9;
        private const int SW_HIDE = 0;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public MainWindow()
        {
            try
            {
                AuthWindow w = new AuthWindow();
                w.ShowDialog();
                InitializeComponent();

                _notifyIcon = new NotifyIcon();
                _notifyIcon.Icon = new System.Drawing.Icon(@"MainIcon.ico");
                _notifyIcon.Visible = true;
                _notifyIcon.DoubleClick += delegate(object sender, EventArgs args)
                {
                    //this.WindowState = WindowState.Normal;
                    //this.Show();
                    //WindowInteropHelper(childWindow).Handle, SW_RESTORE);
                    ShowWindow(new WindowInteropHelper(this).Handle, SW_RESTORE);
                    /*this.Visibility = System.Windows.Visibility.Visible;
                    this.Activate();
                    this.Focus();*/
                };
                MainViewModel.Instance.AudioPlayerVM.PropertyChanged += Instance_PropertyChanged;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                this.Dispatcher.Invoke(() =>
                    {
                        TaskbarItemInfo.ProgressValue = MainViewModel.Instance.AudioPlayerVM.Position;
                    }); ;
            }
            if (e.PropertyName == "IsPlaying")
            {
                this.Dispatcher.Invoke(() =>
                    {
                        if (MainViewModel.Instance.AudioPlayerVM.IsPlaying) { TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal; }
                        else { TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Paused; }
                    });
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) { ShowWindow(new WindowInteropHelper(this).Handle, SW_HIDE); }// this.Hide(); }

            base.OnStateChanged(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
