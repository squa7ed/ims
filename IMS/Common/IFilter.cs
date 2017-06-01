using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common
{
    public interface IFilter<T> : ISelectable where T : BaseEntity
    {
        string Title { get; set; }

        string Name { get; set; }

        int? Count { get; set; }

        IEnumerable<IFilter<T>> Filters { get; set; }

        IEnumerable<T> Result { get; set; }

        Func<T, bool> FilterPredicate { get; set; }
    }
}
