using IMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public partial class Author : BaseEntity
    {
        public string Name { get; set; }
    }

    public partial class Author : IFilter<Relic>
    {
        public string Title { get => "著者"; set { } }

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
                    foreach (var filter in Repository<Author>.Get())
                    {
                        list.Add(filter.Clone() as Author);
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
            set { result = value; Count = value.Count(); NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private Func<Relic, bool> filterPredicate;
        public Func<Relic, bool> FilterPredicate
        {
            get
            {
                if (filterPredicate == null)
                {
                    filterPredicate = x => { return x.Author?.Id == Id; };
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
