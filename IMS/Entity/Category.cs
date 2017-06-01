using IMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IMS.Entity
{
    [Serializable]
    public partial class Category : BaseEntity
    {
        public string Name { get; set; }

        private HashSet<Guid?> relicsId;
        [field: NonSerialized]
        private HashSet<Relic> relics;
        public HashSet<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    if (relicsId != null)
                    {
                        relics = new HashSet<Relic>();
                        foreach (var relicId in relicsId)
                        {
                            relics.Add(Repository<Relic>.Get().FirstOrDefault(x => x.Id == relicId));
                        }
                    }
                }
                return relics;
            }
            set
            {
                relics = value;
                NotifyPropertyChanged();
                if (relicsId == null)
                {
                    relicsId = new HashSet<Guid?>();
                }
                foreach (var relic in relics)
                {
                    relicsId.Add(relic?.Id);
                }
            }
        }
    }

    public partial class Category : IFilter<Relic>
    {
        public string Title { get => "类别"; set { } }

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
                    foreach (var filter in Repository<Category>.Get())
                    {
                        list.Add(filter.Clone() as Category);
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
                    filterPredicate = x => { return x.Category.Id == Id; };
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