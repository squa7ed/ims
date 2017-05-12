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
    class WarehouseManagement : IPlugin
    {
        public string Title => "仓库管理";

        public ImageSource Icon => Application.Current.TryFindResource("IconHome") as BitmapImage;

        private UserControl content;
        public UserControl Content => content ?? (content = new WarehouseCollectionView() {DataContext = new WarehouseCollectionViewModel() });

        public string Description => "新增、修改单位物品、仓库、仓库库位，内部转移物品等。";

        private Brush theme;
        public Brush Theme => theme ?? (theme = new SolidColorBrush(Color.FromArgb(255, 32, 150, 168)));
    }
}
