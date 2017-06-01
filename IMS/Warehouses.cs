using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS
{
    public class Warehouses : ObservableCollection<Storage>
    {
        public Warehouses()
        {
            foreach (var item in Repository<Storage>.Get().Where(x => x.Parent == null))
            {
                Add(item);
            }
        }
    }
}
