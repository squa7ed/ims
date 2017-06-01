using IMS.Common.ViewModels;
using IMS.Entity;
using IMS.Plugins;
using IMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace IMS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IList<IPlugin> Plugins { get; set; }

        private IPlugin plugin;
        public IPlugin CurrentPlugin
        {
            get
            {
                if (plugin == null)
                {
                    CurrentPlugin = Plugins.FirstOrDefault();
                }
                return plugin;
            }
            set
            {
                plugin = value;
                NotifyPropertyChanged();
                object content = value.Content;
                ViewManager.Change(content);
            }
        }

        private object currentContent;
        public object CurrentContent
        {
            get => currentContent;
            set
            {
                NotifyPropertyChanging();
                currentContent = value;
                NotifyPropertyChanged();
            }
        }

        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (CurrentContent is UserControl uc)
            {
                if (uc.DataContext is ViewModelBase vm && uc.DataContext != this)
                {
                    vm.Dispose(sender, e);
                }
            }
        }
    }
}
