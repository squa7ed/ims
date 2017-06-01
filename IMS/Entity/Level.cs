using IMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IMS.Entity
{
    [Serializable]
    public partial class Level : BaseEntity
    {
        public string Name { get; set; }

        [field: NonSerialized]
        private HashSet<Relic> relics;
        public HashSet<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    relics = new HashSet<Relic>(Repository<Relic>.Get().Where(x => x.Level?.Id == Id));
                }
                return relics;
            }
            set { relics = value; NotifyPropertyChanged(); }
        }
    }

    public partial class Level : IFilter<Relic>
    {
        public string Title { get => "文物级别"; set { } }

        [field: NonSerialized]
        private int? count;
        public int? Count
        {
            get
            {
                if (Result.Count() == 0)
                {
                    count = null;
                }
                else
                {
                    count = Result.Count();
                }
                return count;
            }
            set { count = value; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private IEnumerable<IFilter<Relic>> filters;
        public IEnumerable<IFilter<Relic>> Filters
        {
            get
            {
                if (filters == null)
                {
                    var list = new HashSet<IFilter<Relic>>();
                    foreach (var filter in Repository<Level>.Get())
                    {
                        list.Add(filter.Clone() as Level);
                    }
                    filters = list;
                }
                return filters;
            }
            set { filters = value; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private IEnumerable<Relic> result;
        public IEnumerable<Relic> Result
        {
            get
            {
                if (result == null)
                {
                    result = Repository<Relic>.Get().Where(FilterPredicate);
                }
                return result;
            }
            set { result = value; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private Func<Relic, bool> filterPredicate;
        public Func<Relic, bool> FilterPredicate
        {
            get
            {
                if (filterPredicate == null)
                {
                    filterPredicate = x => { return x.Level.Id == Id; };
                }
                return filterPredicate;
            }
            set { filterPredicate = value; NotifyPropertyChanged(); }
        }

        private SelectionChangedEventHandler selectionChanged;
        public event SelectionChangedEventHandler SelectionChanged
        {
            add
            {
                if (selectionChanged == null || !selectionChanged.GetInvocationList().Contains(value))
                {
                    selectionChanged += value;
                }
            }
            remove { selectionChanged -= value; }
        }

        [field: NonSerialized]
        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value; NotifyPropertyChanged(); selectionChanged?.Invoke(this); } }
    }
}
