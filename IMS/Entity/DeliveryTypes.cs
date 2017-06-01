using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public enum DeliveryTypes
    {

        /// <summary>
        /// 提借藏品
        /// </summary>
        Borrow,
        /// <summary>
        /// 向外馆借出
        /// </summary>
        ReturnToOutUnit,
        /// <summary>
        /// 注销藏品
        /// </summary>
        Unregister
    }
}
