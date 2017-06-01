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
        private static Stack<object> contents = new Stack<object>();

        #region Public methods
        public static void Loading()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                loading = true;
                MainWindow.StartLoading();
            });
        }

        public static void Show(object content)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
             {
                 contents.Push(ViewModel.CurrentContent);
                 ViewModel.CurrentContent = content;
             });
        }

        public static void Change(object content)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                var e = new CancelEventArgs(false);
                ViewModel.Dispose(null, e);
                if (e.Cancel)
                {
                    return;
                }
                ViewModel.CurrentContent = content;
            });
        }

        public static void Backward()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                if (contents.Count > 0)
                {
                    var e = new CancelEventArgs(false);
                    ViewModel.Dispose(null, e);
                    if (e.Cancel)
                    {
                        return;
                    }
                    if (contents.Count > 0)
                    {
                        ViewModel.CurrentContent = contents.Pop();
                    }
                }
            });
        }

        public static void LoadingFinished()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
            {
                if (loading)
                {
                    loading = false;
                    MainWindow.StopLoading();
                }
            });
        }


        #endregion


        private static bool loading;

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
