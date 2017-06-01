using IMS.Common.Views;
using IMS.Entity;
using IMS.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            Closing += vm.Dispose;
            vm.PropertyChanging += OnCurrentContentChanging;
            DataContext = vm;
        }

        private void OnCurrentContentChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "CurrentContent")
            {
                (TryFindResource("ContentChangeAnimation") as Storyboard).Begin();
            }
        }

        public void StartLoading()
        {
            VisualStateManager.GoToElementState(this, "Loading", true);
        }

        public void StopLoading()
        {
            VisualStateManager.GoToElementState(this, "Normal", true);
        }
    }
}
