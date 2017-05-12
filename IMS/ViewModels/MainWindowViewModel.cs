using IMS.Common;
using IMS.Entity;
using IMS.Interfaces;
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
    public class MainWindowViewModel : BaseViewModel
    {
        #region ctor
        public MainWindowViewModel()
        {
            Contents = new Stack<object>();
            Plugins = LoadPlugins();
            Loading = false;
        }
        #endregion

        #region Properties
        #region ViewModel
        public List<IPlugin> Plugins { get; private set; }

        private IPlugin plugin;
        public IPlugin CurrentPlugin
        {
            get
            {
                if (plugin == null)
                {
                    CurrentPlugin = Plugins[1];
                }
                return plugin;
            }
            set
            {
                plugin = value;
                NotifyPropertyChanged();
                ViewManager.Change(value.Content);
            }
        }

        private object currentContent;
        public object CurrentContent
        {
            get => currentContent;
            set
            {
                NotifyPropertyChanged("ContentOut");
                currentContent = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ContentIn");
            }
        }

        private bool loading;
        public bool Loading { get => loading; set { loading = value; NotifyPropertyChanged(); } }
        #endregion

        #region View Manager
        public Stack<object> Contents { get; private set; }
        #endregion
        #endregion

        #region Public methods
        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (CurrentContent is UserControl uc)
            {
                if (uc.DataContext is BaseViewModel vm)
                {
                    vm.Dispose(sender, e);
                }
            }
        }
        #endregion

        #region Private methods
        private List<IPlugin> LoadPlugins()
        {
            Log.Verbose("Creating default plugins.");
            var plugins = new List<IPlugin>()
            {
                new WarehouseManagement(),
                new CollectionManagement(),
                new UserManagement()
            };
            if (System.IO.Directory.Exists("Plugins"))
            {
                Log.Verbose("Looking for plugins");
                foreach (var file in System.IO.Directory.GetFiles("Plugins"))
                {
                    Assembly asm = null;
                    Type activator = null;
                    IPlugin plugin = null;
                    try
                    {
                        asm = Assembly.LoadFrom(file);
                        foreach (var type in asm.GetTypes())
                        {
                            foreach (var item in type.GetInterfaces())
                            {
                                if (item.GetType() == typeof(IPlugin))
                                {
                                    activator = type;
                                    break;
                                }
                            }
                            if (activator != null)
                            {
                                break;
                            }
                        }
                        plugin = activator.GetConstructor(new Type[] { }).Invoke(new object[] { }) as IPlugin;
                        var content = plugin.Content;
                        plugins.Add(plugin);
                    }
                    catch (Exception)
                    {
                        Log.Warning("Can't load plugin from file {1}", file);
                    }
                    Log.Verbose("Loaded plugin {0} from assembly {}", plugin.Title, asm.GetName());
                }
            }
            return plugins;
        }
        #endregion
    }
}
