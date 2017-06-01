﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity
{
    [Serializable]
    public class DeliveryDetail : BaseEntity
    {
        private static int _sequence = 1000;

        public string DeliveryId { get => string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), _sequence++); }

        public DateTime Date { get => DateTime.Now; }

        private Guid? deliveryTypeId;
        [field: NonSerialized]
        private DeliveryType deliveryType;
        public DeliveryType DeliveryType { get => deliveryType; set { deliveryType = value; deliveryTypeId = value?.Id; NotifyPropertyChanged(); } }

        private Guid? userId;
        [field: NonSerialized]
        private User user;
        public User User
        {
            get
            {
                if (user == null)
                {
                    user = Repository<User>.Get().FirstOrDefault(x => x.Id == userId);
                }
                return user;
            }
            set { user = value; userId = value?.Id; NotifyPropertyChanged(); }
        }

        private Guid? departmentId;
        [field: NonSerialized]
        private Department department;
        public Department Department
        {
            get
            {
                if (department == null)
                {
                    department = Repository<Department>.Get().FirstOrDefault(x => x.Id == departmentId);
                }
                return department;
            }
            set { department = value; departmentId = value?.Id; NotifyPropertyChanged(); }
        }

        private string remarks;
        public string Remarks { get => remarks; set { remarks = value; NotifyPropertyChanged(); } }

        [field: NonSerialized]
        private IList<Relic> relics;
        public IList<Relic> Relics
        {
            get
            {
                if (relics == null)
                {

                }
                return relics;
            }
            set
            {
                relics = value;
                NotifyPropertyChanged();
            }
        }
    }
}
