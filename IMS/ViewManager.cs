using IMS.Common.Views;
using IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IMS
{
    public class ViewManager
    {

        #region Public methods
        public static void Loading()
        {
            loading = true;
            MainWindow.StartLoading();
        }

        public static void Show(UserControl content)
        {
            StopLoading();
            ViewModel.Contents.Push(ViewModel.CurrentContent);
            ViewModel.CurrentContent = content;
        }

        public static void Change(UserControl content)
        {
            StopLoading();
            var e = new CancelEventArgs(false);
            ViewModel.Dispose(null, e);
            if (e.Cancel)
            {
                return;
            }
            ViewModel.CurrentContent = content;
        }

        public static void Backward()
        {
            StopLoading();
            if (ViewModel.Contents.Count > 0)
            {
                var e = new CancelEventArgs(false);
                ViewModel.Dispose(null, e);
                if (e.Cancel)
                {
                    return;
                }
                ViewModel.CurrentContent = ViewModel.Contents.Pop();
            }
        }




        #endregion


        private static bool loading;

        private static bool showingDialog;

        private static ViewManager loadingWindow;
        private static ViewManager LoadingWindow { get; }

        private static ViewManager dialogWindow;
        private static ViewManager DialogWindow { get; }

        private static void StopLoading()
        {
            if (loading)
            {
                loading = false;
                MainWindow.StopLoading();
            }
        }

        private static MainWindow MainWindow
        {
            get
            {
                return Application.Current.MainWindow as MainWindow;
            }
        }

        private static MainWindowViewModel ViewModel
        {
            get
            {
                return Application.Current.MainWindow.DataContext as MainWindowViewModel;
            }
        }
    }
}
