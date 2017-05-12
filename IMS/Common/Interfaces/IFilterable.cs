using IMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Interfaces
{
    public interface IFilterable<TSource, TFilterType> where TSource : BaseEntity where TFilterType : BaseEntity
    {
        IEnumerable<TSource> SourceList { get; set; }

        string FilterTitle { get; }

        string DisplayMemberPath { get; }

        int Count { get; }

        ObservableCollection<TSource> FilteredItems { get; }
    }
}
