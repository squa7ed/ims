using IMS.Entity;
using IMS.Plugins;
using IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        private MainWindow mainWindow;
        public SplashWindow()
        {
            InitializeComponent();
        }

        private void SplashWindowLoaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = 0,
                To = 100,
                Duration = TimeSpan.FromMilliseconds(1000)
            };
            anim.Completed += ProgressCompleted;
            Progress.BeginAnimation(System.Windows.Controls.Primitives.RangeBase.ValueProperty, anim);
            var relics = Repository<Relic>.Get();
            var vm = new MainWindowViewModel()
            {
                Plugins = LoadPlugins()
            };
            var cp = vm.CurrentPlugin;
            var cc = vm.CurrentContent;
            mainWindow = new MainWindow() { DataContext = vm };
            Application.Current.MainWindow = mainWindow;
        }

        private void ProgressCompleted(object sender, EventArgs e)
        {
            mainWindow.Show();
            Close();
        }

        private IList<IPlugin> LoadPlugins()
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
    }
}
