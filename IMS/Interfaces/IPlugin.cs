using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IMS.Interfaces
{
    public interface IPlugin
    {
        string Title { get; }

        string Description { get; }

        Brush Theme { get; }

        ImageSource Icon { get; }


        UserControl Content { get; }
    }
}
