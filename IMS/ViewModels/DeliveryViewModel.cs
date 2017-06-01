using IMS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using IMS.Entity;
using System.Windows.Input;
using IMS.Common;
using System.Collections;
using System.Collections.ObjectModel;
using IMS.Common.Views;
using System.Windows;

namespace IMS.ViewModels
{
    public class DeliveryViewModel : ViewModelBase
    {
        private bool isDirty = false;
        private bool isSaved = false;

        private Delivery delivery;
        public Delivery Delivery { get => delivery; set { delivery = value; delivery.PropertyChanged += (o, e) => { isDirty = true; }; NotifyPropertyChanged(); } }

        private IList<Relic> relics;
        public IList<Relic> Relics
        {
            get => relics;
            set { relics = value; NotifyPropertyChanged(); }
        }

        private ICommand showSelectionCommand;
        public ICommand ShowSelectionCommand
        {
            get
            {
                if (showSelectionCommand == null)
                {
                    showSelectionCommand = new RelayCommand(
                        () =>
                        {
                            var list = new List<Relic>();
                            switch (Delivery.DeliveryType.Type)
                            {
                                case Entity.DeliveryTypes.Borrow:
                                case Entity.DeliveryTypes.ReturnToOutUnit:
                                    foreach (var item in Repository<Relic>.Get().Where(x => x.StorageAmount > 0))
                                    {
                                        list.Add(item.Clone() as Relic);
                                    }
                                    break;
                                case Entity.DeliveryTypes.Unregister:
                                    foreach (var item in Repository<Relic>.Get())
                                    {
                                        list.Add(item.Clone() as Relic);
                                    }
                                    break;
                            }
                            Relics = list;
                        },
                        () =>
                        {
                            return Delivery.DeliveryType != null;
                        });
                }
                return showSelectionCommand;
            }
        }


        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Backward();
                        });
                }
                return cancelCommand;
            }
        }

        public ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(
                        () =>
                        {
                            switch (Delivery.DeliveryType.Type)
                            {
                                case Entity.DeliveryTypes.Borrow:
                                case Entity.DeliveryTypes.ReturnToOutUnit:
                                    foreach (var item in Delivery.Relics)
                                    {
                                        item.Warehouse.Children.First(x => x.Id == item.Shelf.Id).Relics.Remove(item);
                                        item.Warehouse.Relics.Remove(item);
                                        item.Warehouse = null;
                                        item.Shelf = null;
                                        item.OutStorageAmount = item.Amount;
                                        item.StorageAmount -= item.Amount;
                                        Repository<Relic>.Update(item);
                                    }
                                    break;
                                case Entity.DeliveryTypes.Unregister:
                                    foreach (var item in Delivery.Relics)
                                    {
                                        item.Warehouse.Children.First(x => x.Id == item.Shelf.Id).Relics.Remove(item);
                                        item.Warehouse.Relics.Remove(item);
                                        item.Warehouse = null;
                                        item.Shelf = null;
                                        item.StorageAmount = 0;
                                        item.OutStorageAmount = 0;
                                        item.TotalAmount = 0;
                                        Repository<Relic>.Update(item);
                                    }
                                    break;
                            }

                            Repository<Delivery>.Add(Delivery);
                            Repository<Relic>.SaveChanges();
                            Repository<Delivery>.SaveChanges();
                            isSaved = true;
                            ViewManager.Backward();
                        },
                        () =>
                        {
                            return
                            Delivery.Department != null && Delivery.User != null &&
                            Delivery.DeliveryType != null &&
                            !Delivery.Relics.Any(x => x.Warehouse == null || x.Amount <= 0);
                        });
                }
                return saveCommand;
            }
        }

        public ICommand SaveAndPrintCommand { get { return SaveCommand; } }

        private ICommand confirmSelectionCommand;
        public ICommand ConfirmSelectionCommand
        {
            get
            {
                if (confirmSelectionCommand == null)
                {
                    confirmSelectionCommand = new RelayCommand<IList>(
                        p =>
                        {
                            Delivery.Relics = new HashSet<Relic>();
                            switch (Delivery.DeliveryType.Type)
                            {
                                case Entity.DeliveryTypes.Borrow:
                                case Entity.DeliveryTypes.ReturnToOutUnit:
                                    foreach (var item in p)
                                    {
                                        var relic = item as Relic;
                                        relic.Amount = relic.StorageAmount;
                                        Delivery.Relics.Add(relic);
                                    }
                                    break;
                                case Entity.DeliveryTypes.Unregister:
                                    foreach (var item in p)
                                    {
                                        var relic = item as Relic;
                                        relic.Amount = relic.TotalAmount;
                                        Delivery.Relics.Add(relic);
                                    }
                                    break;
                            }
                        },
                        p =>
                        {
                            return p?.Count > 0;
                        });
                }
                return confirmSelectionCommand;
            }
        }

        public IEnumerable<Storage> Warehouses { get => Repository<Storage>.Get(); }

        public IEnumerable<DeliveryType> DeliveryTypes { get => Repository<DeliveryType>.Get(); }

        public IEnumerable<User> Users { get => Repository<User>.Get(); }

        public IEnumerable<Department> Departments { get => Repository<Department>.Get(); }


        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (!isSaved)
            {
                if (isDirty)
                {
                    Dialog.ShowMessage("是否保存更改？",
                        string.Format("出库单 {0}", string.IsNullOrEmpty(Delivery.DeliveryId) ? string.Empty : " -- " + Delivery.DeliveryId),
                        MessageBoxButton.YesNoCancel, out MessageBoxResult result);
                    switch (result)
                    {
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            break;
                        case MessageBoxResult.Yes:
                            if (SaveCommand.CanExecute(null))
                            {
                                SaveCommand.Execute(null);
                            }
                            else
                            {
                                Dialog.ShowMessage("信息录入不全，无法保存。",
                                    string.Format("出库单 {0}", string.IsNullOrEmpty(Delivery.DeliveryId) ? string.Empty : " -- " + Delivery.DeliveryId),
                                    MessageBoxButton.OK, out result);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
        }
    }
}
