using System;
using System.Collections.Generic;
using System.Linq;

namespace IMS.Entity
{
    [Serializable]
    public class WeightRange : BaseEntity
    {
        public string Name { get; set; }

        private HashSet<Guid?> relicsId;
        [field: NonSerialized]
        private HashSet<Relic> relics;
        public HashSet<Relic> Relics
        {
            get
            {
                if (relics == null)
                {
                    if (relicsId != null)
                    {
                        relics = new HashSet<Relic>();
                        foreach (var relicId in relicsId)
                        {
                            relics.Add(Repository<Relic>.Get().FirstOrDefault(x => x.Id == relicId));
                        }
                    }
                }
                return relics;
            }
            set
            {
                relics = value;
                NotifyPropertyChanged();
                if (relicsId == null)
                {
                    relicsId = new HashSet<Guid?>();
                }
                foreach (var relic in relics)
                {
                    relicsId.Add(relic.Id);
                }
            }
        }
    }
}
