using IMS.Interfaces;
using IMS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IMS.Plugins
{
    class UserManagement : IPlugin
    {
        public string Title => "用户管理";

        public ImageSource Icon => Application.Current.TryFindResource("IconMessenger") as BitmapImage;

        private UserControl content;
        public UserControl Content => content ?? (content = new UserCollectionView() { DataContext = null });

        public string Description => "新增、修改用户，管理用户权限等。";

        private Brush theme;
        public Brush Theme => theme ?? (theme = new SolidColorBrush(Color.FromArgb(255, 210, 109, 0)));
    }
}
