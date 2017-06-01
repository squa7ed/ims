using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMS.Common.ViewModels
{
    public abstract class CollectionViewModelBase<TEntity> : ViewModelBase where TEntity : BaseEntity
    {
        public CollectionViewModelBase()
        {
            Repository<TEntity>.RepositoryChanged += OnRepositoryChanged;
            OnRepositoryChanged(null);
        }

        private int? count;
        public int? Count
        {
            get
            {
                if (count == null)
                {
                    count = Entities.Count();
                }
                return count;
            }
            set { count = value; NotifyPropertyChanged(); }
        }

        private IList<TEntity> entities;
        public virtual IList<TEntity> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = Repository<TEntity>.Get();
                }
                return entities;
            }
            set { entities = value; NotifyPropertyChanged(); Count = value.Count(); }
        }

        private TEntity entity;
        public virtual TEntity Entity { get => entity; set { entity = value; NotifyPropertyChanged(); } }

        public abstract ICommand AddCommand { get; set; }

        public abstract ICommand EditCommand { get; set; }

        protected abstract void OnRepositoryChanged(TEntity entity);
    }
}
