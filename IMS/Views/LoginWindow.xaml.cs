using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool closed = false;
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                DoubleAnimation anim = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(1000)
                };
                BeginAnimation(OpacityProperty, anim);
            };
            Closing += (o, e) =>
            {
                if (!closed)
                {
                    e.Cancel = true;
                    DoubleAnimation anim = new DoubleAnimation()
                    {
                        From = 1,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(1000)
                    };
                    anim.Completed += delegate { closed = true; Close(); };
                    BeginAnimation(OpacityProperty, anim);
                }
            };
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserName.Text) && !string.IsNullOrWhiteSpace(UserPassword.Password))
            {
                Application.Current.Properties.Add("UserName", UserName.Text);
                var splash = new SplashWindow();
                splash.Show();
                Close();
            }
        }
    }
}
