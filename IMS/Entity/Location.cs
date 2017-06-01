using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        private Guid? parentId;
        [field: NonSerialized]
        private Location parent;
        public Location Parent
        {
            get
            {
                if (parent == null)
                {
                    parent = Repository<Location>.Get().FirstOrDefault(x => x.Id == parentId);
                }
                return parent;
            }
            set { parent = value; parentId = value?.Id; NotifyPropertyChanged(); }
        }

        [field: NonSerialized]
        private HashSet<Location> children;
        public HashSet<Location> Children
        {
            get
            {
                if (children == null)
                {
                    children = new HashSet<Location>(Repository<Location>.Get().Where(x => x.Parent?.Id == Id));
                }
                return children;
            }
            set { children = value; NotifyPropertyChanged(); }
        }
    }
}
