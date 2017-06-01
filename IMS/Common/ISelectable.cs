using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IMS.Common
{
    public delegate void SelectionChangedEventHandler(ISelectable sender);

    public interface ISelectable
    {
        event SelectionChangedEventHandler SelectionChanged;

        bool IsSelected { get; set; }
    }
}
