using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public string Password { get; set; }

        private Guid? departmentId;
        [field: NonSerialized]
        private Department department;
        public Department Department
        {
            get
            {
                if (department == null)
                {
                    department = Repository<Department>.Get().FirstOrDefault(x => x.Id == departmentId);
                }
                return department;
            }
            set { department = value; departmentId = value?.Id; NotifyPropertyChanged(); }
        }
    }
}
