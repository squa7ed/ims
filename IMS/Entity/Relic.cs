using IMS.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.IO;

namespace IMS.Entity
{
    [Serializable]
    public partial class Relic : BaseEntity
    {
        private Guid? relicIdTypeId;
        [field: NonSerialized]
        private RelicIdType idType;
        public RelicIdType IdType
        {
            get
            {
                if (idType == null)
                {
                    idType = Repository<RelicIdType>.Get().FirstOrDefault(x => x.Id == relicIdTypeId);
                }
                return idType;
            }
            set
            {
                if (value != null)
                { idType = value; relicIdTypeId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private string relicId;
        public string RelicId { get => relicId; set { relicId = value; NotifyPropertyChanged(); } }

        private string name;
        public string Name { get => name; set { name = value; NotifyPropertyChanged(); } }

        private string originalName;
        public string OriginalName { get => originalName; set { originalName = value; NotifyPropertyChanged(); } }

        public Age Age { get { return GetAge(); } }

        private Guid? rootAgeId;
        [field: NonSerialized]
        private Age rootAge;
        public Age RootAge
        {
            get
            {
                if (rootAge == null)
                {
                    rootAge = Repository<Age>.Get().FirstOrDefault(x => x.Id == rootAgeId);
                }
                return rootAge;
            }
            set
            {
                if (value != null)
                { rootAge = value; rootAgeId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? secondaryAgeId;
        [field: NonSerialized]
        private Age secondaryAge;
        public Age SecondaryAge
        {
            get
            {
                if (secondaryAge == null)
                {
                    secondaryAge = Repository<Age>.Get().FirstOrDefault(x => x.Id == secondaryAgeId);
                }
                return secondaryAge;
            }
            set
            {
                if (value != null)
                { secondaryAge = value; secondaryAgeId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? thirdAgeId;
        [field: NonSerialized]
        private Age thirdAge;
        public Age ThirdAge
        {
            get
            {
                if (thirdAge == null)
                {
                    thirdAge = Repository<Age>.Get().FirstOrDefault(x => x.Id == thirdAgeId);
                }
                return thirdAge;
            }
            set
            {
                if (value != null)
                { thirdAge = value; thirdAgeId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? fourthAgeId;
        [field: NonSerialized]
        private Age fourthAge;
        public Age FourthAge
        {
            get
            {
                if (fourthAge == null)
                {
                    fourthAge = Repository<Age>.Get().FirstOrDefault(x => x.Id == fourthAgeId);
                }
                return fourthAge;
            }
            set
            {
                if (value != null)
                {
                    fourthAge = value; fourthAgeId = value?.Id; NotifyPropertyChanged();
                }
            }
        }

        private Age GetAge()
        {
            if (FourthAge == null)
            {
                if (ThirdAge == null)
                {
                    if (SecondaryAge == null)
                    {
                        return RootAge;
                    }
                    return SecondaryAge;
                }
                return ThirdAge;
            }
            return FourthAge;
        }

        private string specificAge;
        public string SpecificAge { get => specificAge; set { specificAge = value; NotifyPropertyChanged(); } }

        private Guid? categoryId;
        [field: NonSerialized]
        private Category category;
        public Category Category
        {
            get
            {
                if (category == null)
                {
                    category = Repository<Category>.Get().FirstOrDefault(x => x.Id == categoryId);
                }
                return category;
            }
            set
            {
                if (value != null)
                { category = value; categoryId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        public Grain Grain { get { return GetGrain(); } }

        private Guid? rootGrainId;
        [field: NonSerialized]
        private Grain rootGrain;
        public Grain RootGrain
        {
            get
            {
                if (rootGrain == null)
                {
                    rootGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Id == rootGrainId);
                }
                return rootGrain;
            }
            set
            {
                if (value != null)
                { rootGrain = value; rootGrainId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? secondaryGrainId;
        [field: NonSerialized]
        private Grain secondaryGrain;
        public Grain SecondaryGrain
        {
            get
            {
                if (secondaryGrain == null)
                {
                    secondaryGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Id == secondaryGrainId);
                }
                return secondaryGrain;
            }
            set
            {
                if (value != null)
                { secondaryGrain = value; secondaryGrainId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? thirdGrainId;
        [field: NonSerialized]
        private Grain thirdGrain;
        public Grain ThirdGrain
        {
            get
            {
                if (thirdGrain == null)
                {
                    thirdGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Id == thirdGrainId);
                }
                return thirdGrain;
            }
            set
            {
                if (value != null)
                { thirdGrain = value; thirdGrainId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Grain GetGrain()
        {
            if (ThirdGrain == null)
            {
                if (SecondaryGrain == null)
                {
                    return RootGrain;
                }
                return SecondaryGrain;
            }
            return ThirdGrain;
        }

        private double length;
        public double Length { get => length; set { length = value; NotifyPropertyChanged(); } }

        private double width;
        public double Width { get => width; set { width = value; NotifyPropertyChanged(); } }

        private double height;
        public double Height { get => height; set { height = value; NotifyPropertyChanged(); } }

        private Guid? sizeUnitId;
        [field: NonSerialized]
        private Unit sizeUnit;
        public Unit SizeUnit
        {
            get
            {
                if (sizeUnit == null)
                {
                    sizeUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Id == sizeUnitId);
                }
                return sizeUnit;
            }
            set
            {
                if (value != null)
                { sizeUnit = value; sizeUnitId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private string specificSize;
        public string SpecificSize { get => specificSize; set { specificSize = value; NotifyPropertyChanged(); } }


        private Guid? weightRangeId;
        [field: NonSerialized]
        private WeightRange weightRange;
        public WeightRange WeightRange
        {
            get
            {
                if (weightRange == null)
                {
                    weightRange = Repository<WeightRange>.Get().FirstOrDefault(x => x.Id == weightRangeId);
                }
                return weightRange;
            }
            set
            {
                if (value != null)
                { weightRange = value; weightRangeId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private double weight;
        public double Weight { get => weight; set { weight = value; NotifyPropertyChanged(); } }


        private Guid? weightUnitId;
        [field: NonSerialized]
        private Unit weightUnit;
        public Unit WeightUnit
        {
            get
            {
                if (weightUnit == null)
                {
                    weightUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Id == weightUnitId);
                }
                return weightUnit;
            }
            set
            {
                if (value != null)
                { weightUnit = value; weightUnitId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? levelId;
        [field: NonSerialized]
        private Level level;
        public Level Level
        {
            get
            {
                if (level == null)
                {
                    level = Repository<Level>.Get().FirstOrDefault(x => x.Id == levelId);
                }
                return level;
            }
            set
            {
                if (value != null)
                { level = value; levelId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? sourceId;
        [field: NonSerialized]
        private Source source;
        public Source Source
        {
            get
            {
                if (source == null)
                {
                    source = Repository<Source>.Get().FirstOrDefault(x => x.Id == sourceId);
                }
                return source;
            }
            set
            {
                if (value != null)
                { source = value; sourceId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? disabilitylevelId;
        [field: NonSerialized]
        private DisabilityLevel disabilitylevel;
        public DisabilityLevel DisabilityLevel
        {
            get
            {
                if (disabilitylevel == null)
                {
                    disabilitylevel = Repository<DisabilityLevel>.Get().FirstOrDefault(x => x.Id == disabilitylevelId);
                }
                return disabilitylevel;
            }
            set
            {
                if (value != null)
                { disabilitylevel = value; disabilitylevelId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private string disabilityCondition;
        public string DisabilityCondition { get => disabilityCondition; set { disabilityCondition = value; NotifyPropertyChanged(); } }

        private Guid? storingConditionlId;
        [field: NonSerialized]
        private StoringCondition storingCondition;
        public StoringCondition StoringCondition
        {
            get
            {
                if (storingCondition == null)
                {
                    storingCondition = Repository<StoringCondition>.Get().FirstOrDefault(x => x.Id == storingConditionlId);
                }
                return storingCondition;
            }
            set
            {
                if (value != null)
                { storingCondition = value; storingConditionlId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? collectedTimeRangelId;
        [field: NonSerialized]
        private CollectedTimeRange collectedTimeRange;
        public CollectedTimeRange CollectedTimeRange
        {
            get
            {
                if (collectedTimeRange == null)
                {
                    collectedTimeRange = Repository<CollectedTimeRange>.Get().FirstOrDefault(x => x.Id == collectedTimeRangelId);
                }
                return collectedTimeRange;
            }
            set
            {
                if (value != null)
                { collectedTimeRange = value; collectedTimeRangelId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private string collectedYearOfTime;
        public string CollectedYearOfTime { get => collectedYearOfTime; set { collectedYearOfTime = value; NotifyPropertyChanged(); } }

        [field: NonSerialized]
        private ImageSource thumbnail;
        public ImageSource Thumbnail
        {
            get
            {
                if (thumbnail == null)
                {
                    if (Pictures?.Count > 0)
                    {
                        using (var ms = new MemoryStream(Pictures[DefaultPictureIndex]))
                        {
                            //var decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                            //Thumbnail = decoder.Frames[0];
                            var b = new BitmapImage();
                            b.BeginInit();
                            b.CacheOption = BitmapCacheOption.OnLoad;
                            b.DecodePixelWidth = 192;
                            b.StreamSource = ms;
                            b.EndInit();
                            thumbnail = b;
                        }
                    }
                }
                return thumbnail;
            }
            set { thumbnail = value; NotifyPropertyChanged(); }
        }

        private int defaultPictureIndex;
        public int DefaultPictureIndex
        {
            get => defaultPictureIndex;
            set
            {
                if (value < Pictures.Count)
                {
                    defaultPictureIndex = value;
                    NotifyPropertyChanged();
                    using (var ms = new MemoryStream(Pictures[value]))
                    {
                        //var decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        //Thumbnail = decoder.Frames[0];
                        var b = new BitmapImage();
                        b.BeginInit();
                        b.CacheOption = BitmapCacheOption.OnLoad;
                        b.DecodePixelWidth = 128;
                        b.StreamSource = ms;
                        b.EndInit();
                        Thumbnail = b;
                    }
                }
            }
        }

        private IList<byte[]> pictures;
        public IList<byte[]> Pictures { get => pictures; set { pictures = value; NotifyPropertyChanged(); } }

        private string remarks;
        public string Remarks { get => remarks; set { remarks = value; NotifyPropertyChanged(); } }

        private int totalAmount;
        public int TotalAmount { get => totalAmount; set { totalAmount = value; NotifyPropertyChanged(); } }

        private int storageAmount;
        public int StorageAmount { get => storageAmount; set { storageAmount = value; NotifyPropertyChanged(); } }

        private int outStorageAmount;
        public int OutStorageAmount { get => outStorageAmount; set { outStorageAmount = value; NotifyPropertyChanged(); } }

        [field: NonSerialized]
        private int amount;
        public int Amount { get => amount; set { amount = value; NotifyPropertyChanged(); } }

        public int NotStoredAmount { get => TotalAmount - (StorageAmount + OutStorageAmount); }

        private Guid? unitId;
        [field: NonSerialized]
        private Unit unit;
        public Unit Unit
        {
            get
            {
                if (unit == null)
                {
                    unit = Repository<Unit>.Get().FirstOrDefault(x => x.Id == unitId);
                }
                return unit;
            }
            set
            {
                if (value != null)
                { unit = value; unitId = value?.Id; NotifyPropertyChanged(); }
            }
        }

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
            set
            {
                { warehouse = value; warehouseId = value?.Id; NotifyPropertyChanged(); }
            }
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
            set
            {
                { shelf = value; shelfId = value?.Id; NotifyPropertyChanged(); }
            }
        }

        private Guid? authorId;
        [field: NonSerialized]
        private Author author;
        public Author Author
        {
            get
            {
                if (author == null)
                {
                    author = Repository<Author>.Get().FirstOrDefault(x => x.Id == authorId);
                }
                return author;
            }
            set
            {
                { author = value; authorId = value?.Id; NotifyPropertyChanged(); }
            }
        }
    }

    public partial class Relic : ISelectable
    {

        [field: NonSerialized]
        private SelectionChangedEventHandler selectionChanged;
        public event SelectionChangedEventHandler SelectionChanged
        {
            add
            {
                if (selectionChanged == null || !selectionChanged.GetInvocationList().Contains(value))
                {
                    selectionChanged += value;
                }
            }
            remove { selectionChanged -= value; }
        }

        [field: NonSerialized]
        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value; NotifyPropertyChanged(); selectionChanged?.Invoke(this); } }
    }
}
