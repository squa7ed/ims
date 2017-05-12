using System;
using System.Collections.Generic;

namespace IMS.Entity
{
    [Serializable]
    public class Level : BaseEntity
    {
        public string Name { get; set; }

        public List<Relic> Relics { get; set; }

    }
}
