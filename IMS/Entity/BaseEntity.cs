using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace IMS.Entity
{
    [Serializable]
    public abstract class BaseEntity : ICloneable, INotifyPropertyChanged
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            string str = string.Format("{0}:\n", GetType());
            foreach (var prop in GetType().GetProperties())
            {
                str += string.Format("\t{0} : {1}\n", prop.Name, prop.GetMethod.Invoke(this, new object[] { }));
            }
            return str;
        }

        public override bool Equals(object obj)
        {
            return (obj is BaseEntity && (obj as BaseEntity).Id == Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
