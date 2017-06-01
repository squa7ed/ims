using IMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IMS.Entity
{
    [Serializable]
    public partial class Grain : BaseEntity
    {
        public string Name { get; set; }

        private Guid? parentId;
        [field: NonSerialized]
        private Grain parent;
        public Grain Parent
        {
            get
            {
                if (parent == null)
                {
                    parent = Repository<Grain>.Get().FirstOrDefault(x => x.Id == parentId);
                }
                return parent;
            }
            set { parent = value; parentId = value?.Id; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private HashSet<Grain> children;
        public virtual HashSet<Grain> Children
        {
            get
            {
                if (children == null)
                {
                    children = new HashSet<Grain>(Repository<Grain>.Get().Where(x => x.Parent?.Id == Id));
                }
                return children;
            }
            set { children = value; NotifyPropertyChanged(); }
        }
    }


    public partial class Grain : IFilter<Relic>
    {
        public string Title { get => "质地"; set { } }

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

        private IEnumerable<IFilter<Relic>> filters;
        public IEnumerable<IFilter<Relic>> Filters
        {
            get
            {
                if (filters == null)
                {
                    var list = new HashSet<IFilter<Relic>>();
                    foreach (var filter in Repository<Grain>.Get())
                    {
                        list.Add(filter.Clone() as Grain);
                    }
                    filters = list;
                }
                return filters;
            }
            set { filters = value; NotifyPropertyChanged(); }
        }

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

        private Func<Relic, bool> filterPredicate;
        public Func<Relic, bool> FilterPredicate
        {
            get
            {
                if (filterPredicate == null)
                {
                    filterPredicate = x => { return x.Grain.Id == Id; };
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

        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value; NotifyPropertyChanged(); selectionChanged?.Invoke(this); } }
    }
}
