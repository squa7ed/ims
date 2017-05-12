using IMS.Common;
using IMS.Entity;
using IMS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using IMS.Common.Views;
using System.Windows;
using System.Data;
using System.Data.OleDb;

namespace IMS.ViewModels
{
    class RelicCollectionViewModel : BaseViewModel
    {
        private string file;

        public RelicCollectionViewModel()
        {
            Repository<Relic>.RepositoryChanged += (e) => { SetFilters(); };
            SetFilters();
        }

        #region Properties

        public ObservableCollection<FilterItem> FilterList { get; set; }

        private FilterItem filterResult;
        public FilterItem FilterResult
        {
            get => filterResult;

            set { filterResult = value; NotifyPropertyChanged(); }
        }

        public Relic SelectedRelic { get; set; }

        public string FilterText { get; set; }

        #region Commands
        private ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(
                        p =>
                        {
                            ViewManager.Show(new RelicView() { DataContext = new RelicViewModel() { Relic = new Relic() } });
                        });
                }
                return addCommand;
            }
        }

        private ICommand editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(
                        p =>
                        {
                            ViewManager.Show(new RelicView() { DataContext = new RelicViewModel() { Relic = SelectedRelic } });
                        },
                        p => { return SelectedRelic != null; });
                }
                return editCommand;
            }
        }

        //TODO Import
        private ICommand importCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (importCommand == null)
                {
                    importCommand = new RelayCommand(
                      p =>
                      {
                      });
                }
                return importCommand;
            }
        }
        //TODO Repair
        public ICommand RepairCommand { get; set; }

        //TODO Replicate
        public ICommand ReplicateCommand { get; set; }
        #endregion

        #endregion

        #region Private Methods
        private void SetFilters()
        {
            if (FilterList == null)
            {
                FilterList = new ObservableCollection<FilterItem>();
            }
            FilterList.Clear();
            var relics = Repository<Relic>.Get();
            FilterList.Add(new FilterItem()
            {
                Title = "所有藏品",
                Count = relics.Count,
                Result = relics
            });
            foreach (var item in Repository<Category>.Get())
            {
                if (relics.Any(x => x.Category.Id == item.Id))
                {
                    FilterList.Add(
                        new FilterItem()
                        {
                            Title = item.Name,
                            Count = relics.Count(x => x.Category.Id == item.Id),
                            Result = new ObservableCollection<Relic>(relics.Where(x => x.Category.Id == item.Id))
                        });
                }
            }
            FilterResult = FilterList[0];
        }
        #endregion

        public override void Dispose(object sender, CancelEventArgs e)
        {
        }
    }

    public class FilterItem
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public int? Count { get; set; }

        public ObservableCollection<Relic> Result { get; set; }
    }
}
