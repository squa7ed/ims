using IMS.Interfaces;
using IMS.ViewModels;
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
    class CollectionManagement : IPlugin
    {
        public string Title => "藏品管理";

        public ImageSource Icon => Application.Current.TryFindResource("IconStar") as BitmapImage;

        private UserControl content;
        public UserControl Content => content ?? (content = new RelicCollectionView() { DataContext = new RelicCollectionViewModel() });

        public string Description => "对藏品进行新增、修改、导入、修复、仿制等。";

        private Brush theme;
        public Brush Theme => theme ?? (theme = new SolidColorBrush(Color.FromArgb(255, 31, 132, 203)));
    }
}
