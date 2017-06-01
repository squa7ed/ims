using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IMS.Common
{
    public interface IFilterable<T> where T : BaseEntity
    {
        IEnumerable<IFilter<T>> FilterList { get; set; }

        IFilter<T> CurrentFilter { get; set; }

        int? Count { get; set; }

        ICommand ResetFilterCommand { get; }

        void Filter();
    }
}
