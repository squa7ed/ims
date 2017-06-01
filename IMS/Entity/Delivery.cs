using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IMS.Entity
{
    [Serializable]
    public class Delivery : BaseEntity
    {
        private Guid? deliveryDetailId;
        [field: NonSerialized]
        private DeliveryDetail deliveryDetail;
        public DeliveryDetail DeliveryDetail
        {
            get
            {
                if (deliveryDetail == null)
                {
                    deliveryDetail = Repository<DeliveryDetail>.Get().FirstOrDefault(x => x.Id == deliveryDetailId);
                }
                return deliveryDetail;
            }
            set { deliveryDetail = value; deliveryDetailId = value?.Id; NotifyPropertyChanged(); }
        }

        private Guid? relicId;
        [field: NonSerialized]
        private Relic relic;
        public Relic Relic
        {
            get
            {
                if (relic == null)
                {
                    relic = Repository<Relic>.Get().FirstOrDefault(x => x.Id == relicId);
                }
                return relic;
            }
            set { relic = value; relicId = value?.Id; NotifyPropertyChanged(); }
        }

        private int count;
        public int Count { get => count; set { count = value; NotifyPropertyChanged(); } }

        private Guid? warehouseId;
        [field: NonSerialized]
        private Storage warehouse;
        public Storage Warehouse
        {
            get
            {
                if (warehouse == null)
                {
                    warehouse = Repository<Storage>.Get().FirstOrDefault(x => x.Id == warehouseId);
                }
                return warehouse;
            }
            set { warehouse = value; warehouseId = value?.Id; NotifyPropertyChanged(); }
        }

        private Guid? shelfId;
        [field: NonSerialized]
        private Storage shelf;
        public Storage Shelf
        {
            get
            {
                if (shelf == null)
                {
                    shelf = Repository<Storage>.Get().FirstOrDefault(x => x.Id == shelfId);
                }
                return shelf;
            }
            set { shelf = value; shelfId = value?.Id; NotifyPropertyChanged(); }
        }
    }
}
