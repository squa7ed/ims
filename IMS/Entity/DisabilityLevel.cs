using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IMS.Entity
{
    [Serializable]
    public class DisabilityLevel : BaseEntity
    {
        public string Name { get; set; }

        [field: NonSerialized]
        private HashSet<Relic> relics;
        public virtual HashSet<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    relics = new HashSet<Relic>(Repository<Relic>.Get().Where(x => x.DisabilityLevel?.Id == Id));
                }
                return relics;
            }
            set { relics = value; NotifyPropertyChanged(); }
        }

    }
}
