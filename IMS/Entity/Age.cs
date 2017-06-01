using IMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace IMS.Entity
{
    [Serializable]
    public partial class Age : BaseEntity
    {
        public string Name { get; set; }

        private Guid? parentId;
        [field: NonSerialized]
        private Age parent;
        public Age Parent
        {
            get
            {
                if (parent == null)
                {
                    parent = Repository<Age>.Get().FirstOrDefault(x => x.Id == parentId);
                }
                return parent;
            }
            set { parent = value; parentId = value?.Id; }
        }

        [field: NonSerialized]
        private HashSet<Age> children;
        public HashSet<Age> Children
        {
            get
            {
                if (children == null)
                {
                    children = new HashSet<Age>(Repository<Age>.Get().Where(x => x.Parent?.Id == Id));
                }
                return children;
            }
            set { children = value; }
        }

        private HashSet<Guid?> relicsId;
        [field: NonSerialized]
        private IList<Relic> relics;
        public IList<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    if (relicsId != null)
                    {
                        relics = new List<Relic>();
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
                    relicsId.Add(relic.Id);
                }
            }
        }
    }

    public partial class Age : IFilter<Relic>
    {
        public string Title { get => "年代"; set { } }

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
                    foreach (var filter in Repository<Age>.Get())
                    {
                        list.Add(filter.Clone() as Age);
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
                    filterPredicate = x => { return x.Age.Id == Id; };
                }
                return filterPredicate;
            }
            set { filterPredicate = value; NotifyPropertyChanged(); }
        }



        //[field: NonSerialized]
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

        //[field: NonSerialized]
        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value; NotifyPropertyChanged(); selectionChanged?.Invoke(this); } }
    }
}
