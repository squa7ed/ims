using System;
using System.Collections.Generic;

namespace IMS.Entity
{
    [Serializable]
    public class StoringCondition : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<Relic> Relics { get; set; }

    }
}
