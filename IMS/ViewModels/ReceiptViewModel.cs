using IMS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using IMS.Entity;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IMS.Common.Views;
using System.Windows;
using IMS.Common;
using System.Windows.Controls;
using System.Collections;

namespace IMS.ViewModels
{
    public class ReceiptViewModel : ViewModelBase
    {
        private bool isDirty = false;
        private bool isSaved = false;

        private ReceiptDetail receiptDetail;
        public ReceiptDetail ReceiptDetail { get => receiptDetail; set { receiptDetail = value; receiptDetail.PropertyChanged += (o, e) => { isDirty = true; }; NotifyPropertyChanged(); } }

        public Storage DefaultWarehouse { get => Repository<Storage>.Get().FirstOrDefault(); }

        private IList<Relic> relics;
        public IList<Relic> Relics
        {
            get => relics;
            set { relics = value; NotifyPropertyChanged(); }
        }

        private IList<Receipt> selectedEntities;
        public IList<Receipt> SelectedEntities
        {
            get
            {
                if (selectedEntities == null)
                {
                    selectedEntities = new List<Receipt>(Repository<Receipt>.Get().Where(x => x.ReceiptDetail.Equals(ReceiptDetail)));
                }
                return selectedEntities;
            }
            set { selectedEntities = value; NotifyPropertyChanged(); }
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
                            switch (ReceiptDetail.ReceiptType.Type)
                            {
                                case Entity.ReceiptTypes.NewReceipt:
                                    foreach (var item in Repository<Relic>.Get().Where(x => x.NotStoredAmount > 0))
                                    {
                                        list.Add(item.Clone() as Relic);
                                    }
                                    break;
                                case Entity.ReceiptTypes.BorrowFromOuterUnit:
                                    foreach (var item in Repository<Relic>.Get().Where(x => x.StorageAmount > 0))
                                    {
                                        list.Add(item.Clone() as Relic);
                                    }
                                    break;
                                case Entity.ReceiptTypes.Return:
                                    foreach (var item in Repository<Relic>.Get().Where(x => x.StorageAmount > 0))
                                    {
                                        list.Add(item.Clone() as Relic);
                                    }
                                    break;
                            }
                            Relics = list;
                        },
                        () =>
                        {
                            return ReceiptDetail.ReceiptType != null && DefaultWarehouse != null;
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
                            ViewManager.Loading();
                            {
                                foreach (var item in SelectedEntities)
                                {
                                    Repository<Receipt>.Add(item);
                                }
                                Repository<ReceiptDetail>.Add(ReceiptDetail);
                                Repository<ReceiptDetail>.SaveChanges();
                                Repository<Receipt>.SaveChanges();
                            }
                            isSaved = true;
                            ViewManager.Backward();
                            ViewManager.LoadingFinished();
                        },
                        () =>
                        {
                            return
                            ReceiptDetail.Department != null && ReceiptDetail.User != null &&
                            ReceiptDetail.ReceiptType != null &&
                            !SelectedEntities.Any(x => x.Warehouse == null || x.Count <= 0);
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
                            var list = new List<Receipt>(p.Count);
                            switch (ReceiptDetail.ReceiptType.Type)
                            {
                                case Entity.ReceiptTypes.NewReceipt:
                                    foreach (var item in p)
                                    {
                                        var relic = item as Relic;
                                        //relic.Warehouse = DefaultWarehouse;
                                        //relic.Shelf = relic.Warehouse.Children.First();
                                        var receipt = new Receipt()
                                        {
                                            ReceiptDetail = ReceiptDetail,
                                            Relic = relic,
                                            Warehouse = DefaultWarehouse,
                                            Shelf = DefaultWarehouse.Children.FirstOrDefault(),
                                            Count = relic.NotStoredAmount
                                        };
                                        list.Add(receipt);
                                    }
                                    break;
                                case Entity.ReceiptTypes.BorrowFromOuterUnit:
                                case Entity.ReceiptTypes.Return:
                                    foreach (var item in p)
                                    {
                                        var relic = item as Relic;
                                        var receipt = new Receipt()
                                        {
                                            ReceiptDetail = ReceiptDetail,
                                            Relic = relic,
                                            Warehouse = DefaultWarehouse,
                                            Shelf = DefaultWarehouse.Children.FirstOrDefault(),
                                            Count = relic.OutStorageAmount
                                        };
                                        list.Add(receipt);
                                    }
                                    break;
                            }
                            SelectedEntities = list;
                        },
                        p =>
                        {
                            return p?.Count > 0;
                        });
                }
                return confirmSelectionCommand;
            }
        }

        public IEnumerable<Storage> Warehouses { get => Repository<Storage>.Get().Where(x => x.Parent == null); }

        public IEnumerable<ReceiptType> ReceiptTypes { get => Repository<ReceiptType>.Get(); }

        public IEnumerable<User> Users { get => Repository<User>.Get(); }

        public IEnumerable<Department> Departments { get => Repository<Department>.Get(); }


        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (!isSaved)
            {
                if (isDirty)
                {
                    Dialog.ShowMessage("是否保存更改？",
                        string.Format("入库单 {0}", string.IsNullOrEmpty(ReceiptDetail.ReceiptId) ? string.Empty : " -- " + ReceiptDetail.ReceiptId),
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
                                    string.Format("入库单 {0}", string.IsNullOrEmpty(ReceiptDetail.ReceiptId) ? string.Empty : " -- " + ReceiptDetail.ReceiptId),
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
