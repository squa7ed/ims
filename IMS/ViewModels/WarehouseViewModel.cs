using IMS.Common.ViewModels;
using System.Windows.Input;
using System.ComponentModel;
using IMS.Entity;
using System;
using System.Collections.ObjectModel;
using IMS.Common;
using IMS.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace IMS.ViewModels
{
    public partial class WarehouseViewModel
    {
        public WarehouseViewModel() : base()
        {
            Repository<Receipt>.RepositoryChanged += (o) => { ReceiptDetails = Repository<ReceiptDetail>.Get(); };
            Repository<Delivery>.RepositoryChanged += (o) => { Deliveries = Repository<Delivery>.Get(); };
        }

        private IEnumerable<ReceiptDetail> receiptDetails;
        public IEnumerable<ReceiptDetail> ReceiptDetails
        {
            get
            {
                if (receiptDetails == null)
                {
                    receiptDetails = Repository<ReceiptDetail>.Get();
                }
                return receiptDetails;
            }
            set { receiptDetails = value; NotifyPropertyChanged(); }
        }

        public ReceiptDetail ReceiptDetail { get; set; }

        public IEnumerable<Delivery> deliveries;
        public IEnumerable<Delivery> Deliveries
        {
            get
            {
                if (deliveries == null)
                {
                    deliveries = Repository<Delivery>.Get();
                }
                return deliveries;
            }
            set { deliveries = value; NotifyPropertyChanged(); }
        }

        public Delivery Delivery { get; set; }

        private ICommand receiptCommand;
        public ICommand ReceiptCommand
        {
            get
            {
                if (receiptCommand == null)
                {
                    receiptCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new ReceiptView() { DataContext = new ReceiptViewModel() { ReceiptDetail = new ReceiptDetail(), IsEditable = true } });
                        });
                }
                return receiptCommand;
            }
        }

        private ICommand viewReceiptCommand;
        public ICommand ViewReceiptCommand
        {
            get
            {
                if (viewReceiptCommand == null)
                {
                    viewReceiptCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new ReceiptView() { DataContext = new ReceiptViewModel() { ReceiptDetail = ReceiptDetail, IsEditable = false } });
                        });
                }
                return viewReceiptCommand;
            }
        }

        private ICommand viewDeliveryCommand;
        public ICommand ViewDeliveryCommand
        {
            get
            {
                if (viewDeliveryCommand == null)
                {
                    viewDeliveryCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new DeliveryView() { DataContext = new DeliveryViewModel() { Delivery = Delivery, IsEditable = false } });
                        });
                }
                return viewDeliveryCommand;
            }
        }

        private ICommand deliveryCommand;
        public ICommand DeliveryCommand
        {
            get
            {
                if (deliveryCommand == null)
                {
                    deliveryCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new DeliveryView() { DataContext = new DeliveryViewModel() { Delivery = new Delivery() } });
                        });
                }
                return deliveryCommand;
            }
        }

        public override ICommand AddCommand { get; set; }
        public override ICommand EditCommand { get; set; }
    }

    public partial class WarehouseViewModel : IFilterable<Relic>
    {
        private Dictionary<string, HashSet<IFilter<Relic>>> currentFilters = new Dictionary<string, HashSet<IFilter<Relic>>>();

        private IEnumerable<IFilter<Relic>> filterList;
        public IEnumerable<IFilter<Relic>> FilterList
        {
            get
            {
                if (filterList == null)
                {
                    filterList = CreateFilters();
                }
                return filterList;
            }
            set { filterList = value; NotifyPropertyChanged(); }
        }

        private ICommand resetCommand;
        public ICommand ResetFilterCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand(
                       async () =>
                       {
                           ViewManager.Loading();
                           if (currentFilters.Count > 0)
                           {
                               await Task.Run(() =>
                               {
                                   currentFilters.Clear();
                                   var list = CreateFilters();
                                   Application.Current.Dispatcher.BeginInvoke((Action)delegate { FilterList = list; });
                               });
                           }
                           Filter();
                           ViewManager.LoadingFinished();
                       });
                }
                return resetCommand;
            }
        }

        private string filterText;
        public string FilterText { get => filterText; set { filterText = value; NotifyPropertyChanged(); } }

        private IFilter<Relic> currentFilter;
        public IFilter<Relic> CurrentFilter
        {
            get => currentFilter;
            set
            {
                if (currentFilter != null && value != null && currentFilter.Title == value.Title)
                {
                    currentFilter.IsSelected = false;
                }
                currentFilter = value;
                if (value != null)
                {
                    currentFilter.IsSelected = true;
                }
                NotifyPropertyChanged();
            }
        }

        public void Filter()
        {
            var mEntities = new List<Relic>(Repository<Relic>.Get());
            string mFilterText = string.Empty;
            int cnt = 0;
            foreach (var filters in currentFilters.Values)
            {
                var list = new HashSet<Relic>();
                foreach (var filter in filters)
                {
                    list.UnionWith(mEntities.Where(filter.FilterPredicate));
                    mFilterText += filter.Name + "; ";
                    cnt++;
                }
                mEntities.Intersect(list);
            }
            if (mFilterText.EndsWith("; "))
            {
                mFilterText = mFilterText.Remove(mFilterText.Length - 2);
            }
            Application.Current.Dispatcher.BeginInvoke((Action)delegate { Entities = mEntities; FilterText = mFilterText; });
        }

        private IEnumerable<IFilter<Relic>> CreateFilters()
        {
            var list = new HashSet<IFilter<Relic>>() { new Category(), new Level(), new Grain(), new Age(), new Source() };
            foreach (var filters in list)
            {
                foreach (var filter in filters.Filters)
                {
                    filter.SelectionChanged += async (o) =>
                    {
                        ViewManager.Loading();
                        await Task.Run(() =>
                        {
                            if (filter.IsSelected)
                            {
                                if (!currentFilters.ContainsKey(filter.Title))
                                {
                                    currentFilters.Add(filter.Title, new HashSet<IFilter<Relic>>());
                                }
                                currentFilters[filter.Title].Add(filter);
                            }
                            else
                            {
                                if (CurrentFilter == filter)
                                {
                                    CurrentFilter = null;
                                }
                                if (currentFilters.ContainsKey(filter.Title))
                                {
                                    currentFilters[filter.Title].Remove(filter);
                                    if (currentFilters[filter.Title].Count == 0)
                                    {
                                        currentFilters.Remove(filter.Title);
                                    }
                                }
                            }
                            Filter();
                        });
                        ViewManager.LoadingFinished();
                    };
                }
            }
            return list;
        }
    }

    public partial class WarehouseViewModel : CollectionViewModelBase<Relic>
    {

        protected override void OnRepositoryChanged(Relic entity)
        {
            Entities = Repository<Relic>.Get();
            ResetFilterCommand.Execute(null);
        }

        public override void Dispose(object sender, CancelEventArgs e)
        {
        }
    }
}
