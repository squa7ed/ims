using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class Department : BaseEntity
    {
        public string Name { get; set; }

        [field: NonSerialized]
        private HashSet<User> users;
        public virtual HashSet<User> Users
        {
            get
            {
                if (users == null)
                {
                    users = new HashSet<User>(Repository<User>.Get().Where(x => x.Department?.Id == Id));
                }
                return users;
            }
            set
            {
                if (value != null)
                {
                    users = value;
                    foreach (var user in users)
                    {
                        user.Department = this;
                    }
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
