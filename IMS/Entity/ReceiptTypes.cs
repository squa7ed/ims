using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public enum ReceiptTypes
    {
        /// <summary>
        /// 新增入库
        /// </summary>
        NewReceipt,
        /// <summary>
        /// 借出藏品归还
        /// </summary>
        Return,
        /// <summary>
        /// 从外借进藏品
        /// </summary>
        BorrowFromOuterUnit
    }
}
