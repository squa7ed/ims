using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class Storage : BaseEntity
    {
        public string Name { get; set; }

        private Guid? locationId;
        [field: NonSerialized]
        private Location location;
        public Location Location
        {
            get
            {
                if (location == null)
                {
                    location = Repository<Location>.Get().FirstOrDefault(x => x.Id == locationId);
                }
                return location;
            }
            set { location = value; locationId = value?.Id; NotifyPropertyChanged(); }
        }

        private Guid? parentId;
        [field: NonSerialized]
        private Storage parent;
        public Storage Parent
        {
            get
            {
                if (parent == null)
                {
                    parent = Repository<Storage>.Get().FirstOrDefault(x => x.Id == parentId);
                }
                return parent;
            }
            set { parent = value; parentId = value?.Id; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private HashSet<Storage> children;
        public HashSet<Storage> Children
        {
            get
            {
                if (children == null)
                {
                    children = new HashSet<Storage>(Repository<Storage>.Get().Where(x => x.Parent?.Id == Id));
                }
                return children;
            }
            set { children = value; }
        }

        private HashSet<Guid?> relicsId;
        [field: NonSerialized]
        private HashSet<Relic> relics;
        public HashSet<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    relics = new HashSet<Relic>();
                    if (relicsId != null)
                    {
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
}
