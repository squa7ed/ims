using IMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace IMS.ViewModels
{
    class WarehouseCollectionViewModel : BaseViewModel
    {
        private List<string> items;
        public List<string> Items
        {
            get
            {
                if (items == null)
                {
                    items = new List<string>();
                    foreach (var item in Application.Current.Resources.Keys)
                    {
                        items.Add(item.ToString());
                    }
                    foreach (var item in Application.Current.Resources.MergedDictionaries)
                    {
                        foreach (var key in item.Keys)
                        {
                            items.Add(key.ToString());
                        }
                    }
                }
                return items;
            }
        }

        private ICommand zoomInCommand;
        public ICommand ZoomInCommand
        {
            get
            {
                if (zoomInCommand == null)
                {
                    zoomInCommand = new RelayCommand(
                        p => { });
                }
                return zoomInCommand;
            }
        }

        private ICommand zoomOutCommand;
        public ICommand ZoomOutCommand
        {
            get
            {
                if (zoomOutCommand == null)
                {
                    zoomOutCommand = new RelayCommand(
                        p =>
                        {

                        });
                }
                return zoomOutCommand;
            }
        }

        public override void Dispose(object sender, CancelEventArgs e)
        {

        }
    }
}
