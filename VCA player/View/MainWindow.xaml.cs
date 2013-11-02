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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VCA_player.Model;
using System.Media;
using System.Web;
using System.Net;
using VCA_player.ViewModel;

namespace VCA_player.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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

                NotifyIcon ni = new NotifyIcon();
                ni.Icon = new System.Drawing.Icon(@"MainIcon.ico");
                ni.Visible = true;
                ni.DoubleClick += delegate(object sender, EventArgs args)
                {
                    this.WindowState = WindowState.Normal;
                    this.Show();
                };
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }
    }
}
