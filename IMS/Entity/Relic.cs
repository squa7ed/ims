using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IMS.Entity
{
    [Serializable]
    public class Relic : BaseEntity
    {
        public Relic() : base()
        {
            Pictures = new ObservableCollection<string>();
            Pictures.CollectionChanged += PictureChanged;
        }

        private void PictureChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Pictures");
        }

        private RelicIdType idType;
        public RelicIdType IdType { get => idType; set { idType = value; NotifyPropertyChanged(); } }

        private string relicId;
        public string RelicId { get => relicId; set { relicId = value; NotifyPropertyChanged(); } }

        private string name;
        public string Name { get => name; set { name = value; NotifyPropertyChanged(); } }

        private string originalName;
        public string OriginalName { get => originalName; set { originalName = value; NotifyPropertyChanged(); } }

        public Age Age { get { return GetAge(); } }

        private Age rootAge;
        public Age RootAge { get => rootAge; set { rootAge = value; NotifyPropertyChanged(); } }

        private Age secondaryAge;
        public Age SecondaryAge { get => secondaryAge; set { secondaryAge = value; NotifyPropertyChanged(); } }

        private Age thirdAge;
        public Age ThirdAge { get => thirdAge; set { thirdAge = value; NotifyPropertyChanged(); } }

        private Age fourthAge;
        public Age FourthAge { get => fourthAge; set { fourthAge = value; NotifyPropertyChanged(); } }

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

        private Category category;
        public Category Category { get => category; set { category = value; NotifyPropertyChanged(); } }

        public Grain Grain { get { return GetGrain(); } }



        private Grain rootGrain;
        public Grain RootGrain { get => rootGrain; set { rootGrain = value; NotifyPropertyChanged(); } }

        private Grain secondaryGrain;
        public Grain SecondaryGrain { get => secondaryGrain; set { secondaryGrain = value; NotifyPropertyChanged(); } }

        private Grain thirdGrain;
        public Grain ThirdGrain { get => thirdGrain; set { thirdGrain = value; NotifyPropertyChanged(); } }

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

        private int amount;
        public int Amount { get => amount; set { amount = value; NotifyPropertyChanged(); } }

        private double length;
        public double Length { get => length; set { length = value; NotifyPropertyChanged(); } }

        private double width;
        public double Width { get => width; set { width = value; NotifyPropertyChanged(); } }

        private double height;
        public double Height { get => height; set { height = value; NotifyPropertyChanged(); } }

        private string sizeUnit;
        public string SizeUnit { get => sizeUnit; set { sizeUnit = value; NotifyPropertyChanged(); } }

        private string specificSize;
        public string SpecificSize { get => specificSize; set { specificSize = value; NotifyPropertyChanged(); } }

        private WeightRange weightRange;
        public WeightRange WeightRange { get => weightRange; set { weightRange = value; NotifyPropertyChanged(); } }

        private double weight;
        public double Weight { get => weight; set { weight = value; NotifyPropertyChanged(); } }

        private string weightUnit;
        public string WeightUnit { get => weightUnit; set { weightUnit = value; NotifyPropertyChanged(); } }

        private Level level;
        public Level Level { get => level; set { level = value; NotifyPropertyChanged(); } }

        private Source source;
        public Source Source { get => source; set { source = value; NotifyPropertyChanged(); } }

        private DisabilityLevel disabilitylevel;
        public DisabilityLevel DisabilityLevel { get => disabilitylevel; set { disabilitylevel = value; NotifyPropertyChanged(); } }

        private DisabilityCondition disabilityCondition;
        public DisabilityCondition DisabilityCondition { get => disabilityCondition; set { disabilityCondition = value; NotifyPropertyChanged(); } }

        private StoringCondition storingCondition;
        public StoringCondition StoringCondition { get => storingCondition; set { storingCondition = value; NotifyPropertyChanged(); } }

        private CollectedTimeRange collectedTimeRange;
        public CollectedTimeRange CollectedTimeRange { get => collectedTimeRange; set { collectedTimeRange = value; NotifyPropertyChanged(); } }

        private string collectedYearOfTime;
        public string CollectedYearOfTime { get => collectedYearOfTime; set { collectedYearOfTime = value; NotifyPropertyChanged(); } }

        private ObservableCollection<string> pictures;
        public ObservableCollection<string> Pictures { get => pictures; set { pictures = value; NotifyPropertyChanged(); } }

        private string remarks;
        public string Remarks { get => remarks; set { remarks = value; NotifyPropertyChanged(); } }

    }
}
