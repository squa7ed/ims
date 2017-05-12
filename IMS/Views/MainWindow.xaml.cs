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
        private Storyboard fadeOut;
        private Storyboard fadeIn;
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            Closing += vm.Dispose;
            vm.PropertyChanged += OnContentChange;
            DataContext = vm;
            fadeIn = Application.Current.TryFindResource("FadeIn") as Storyboard;
            fadeOut = Application.Current.TryFindResource("FadeOut") as Storyboard;
        }

        public void StartLoading()
        {
            Panel.SetZIndex(LoadingRoot, 1);
            (TryFindResource("LoadingStartAnimation") as Storyboard).Begin();
            (TryFindResource("LoadingIndivcatorAnimation") as Storyboard).Begin();
        }

        public void StopLoading()
        {
            (TryFindResource("LoadingStopAnimation") as Storyboard).Begin();
            (TryFindResource("LoadingIndivcatorAnimation") as Storyboard).Remove();
            Panel.SetZIndex(LoadingRoot, -1);
        }

        private void OnContentChange(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ContentOut":
                    fadeOut.Begin(ContentRoot);
                    break;
                case "ContentIn":
                    fadeIn.Begin(ContentRoot);
                    break;
                default:
                    break;
            }
        }
    }
}
