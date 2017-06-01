using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class ReceiptType : BaseEntity
    {
        public string Name { get; set; }

        public ReceiptTypes Type { get; set; }
    }
}
