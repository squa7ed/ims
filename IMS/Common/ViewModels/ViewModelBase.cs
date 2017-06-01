using IMS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IMS.Common.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private bool? isEditable;
        public virtual bool? IsEditable
        {
            get
            {
                if (isEditable == null)
                {
                    isEditable = true;
                }
                return isEditable;
            }
            set { isEditable = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void NotifyPropertyChanging([CallerMemberName] string name = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }

        public abstract void Dispose(object sender, CancelEventArgs e);
    }
}
