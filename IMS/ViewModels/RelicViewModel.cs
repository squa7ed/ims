using IMS.Common;
using IMS.Entity;
using IMS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using IMS.Views;
using System.ComponentModel;
using System.Windows;
using IMS.Common.Views;

namespace IMS.ViewModels
{
    class RelicViewModel : ViewModelBase
    {
        private bool saved;
        private bool dirty;
        private bool pictureChanged;
        #region ctor
        public RelicViewModel()
        {
            saved = false;
            dirty = false;
        }
        #endregion

        #region Properties
        private Relic relic;
        public Relic Relic { get => relic; set { relic = value; NotifyPropertyChanged(); Copy = relic.Clone() as Relic; ImageIndex = relic.DefaultPictureIndex; } }

        private Relic copy;
        public Relic Copy
        {
            get => copy; set { copy = value; NotifyPropertyChanged(); copy.PropertyChanged += (o, e) => { dirty = true; }; }
        }

        private ObservableCollection<BitmapSource> pictures;
        public ObservableCollection<BitmapSource> Pictures
        {
            get
            {
                if (pictures == null)
                {
                    pictures = new ObservableCollection<BitmapSource>();
                    if (Copy.Pictures != null)
                    {
                        foreach (var item in Copy.Pictures)
                        {
                            using (var ms = new MemoryStream(item))
                            {
                                var decoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                                var bitmap = new WriteableBitmap(decoder.Frames[0]);
                                bitmap.Freeze();
                                pictures.Add(bitmap);
                            }
                        } /**/
                    }
                    pictures.CollectionChanged += (o, e) => { dirty = true; pictureChanged = true; };
                }
                return pictures;
            }
            set { pictures = value; NotifyPropertyChanged(); }
        }


        private int imageIndex;
        public int ImageIndex
        {
            get => imageIndex;
            set
            {
                if (value < Pictures.Count)
                {
                    imageIndex = value;
                    NotifyPropertyChanged(); ;
                    CurrentImage = Pictures[value];
                }
            }
        }

        private BitmapSource currentImage;
        public BitmapSource CurrentImage
        {
            get => currentImage;
            set { currentImage = value; NotifyPropertyChanged(); }
        }

        #region Commands
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(
                        () =>
                        {
                            if (dirty)
                            {
                                if (pictureChanged)
                                {
                                    Copy.Pictures.Clear();
                                    foreach (var item in Pictures)
                                    {
                                        var encoder = new JpegBitmapEncoder();
                                        encoder.Frames.Add(BitmapFrame.Create(item));
                                        using (var ms = new MemoryStream())
                                        {
                                            encoder.Save(ms);
                                            Copy.Pictures.Add(ms.ToArray());
                                        }
                                    }
                                }
                                if (Repository<Relic>.Get().Any(x => x.Id == Copy.Id))
                                {
                                    Repository<Relic>.Update(Copy);
                                }
                                else
                                {
                                    Repository<Relic>.Add(Copy);
                                }
                            }
                            saved = true;
                            ViewManager.Backward();
                        },
                        () =>
                        {
                            var hasDuplicate = false;
                            if (Repository<Relic>.Get().Any(x => x.Id == Copy.Id))
                            {
                                if (Relic.RelicId != Copy.RelicId)
                                {
                                    hasDuplicate = Repository<Relic>.Get().Any(x => x.RelicId == Copy.RelicId);
                                }
                            }
                            else
                            {
                                hasDuplicate = Repository<Relic>.Get().Any(x => x.RelicId == Copy.RelicId);
                            }
                            return !hasDuplicate && Copy.CanSave();
                        });
                }
                return saveCommand;
            }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Backward();
                        });
                }
                return cancelCommand;
            }
        }

        private ICommand nextImageCommand;
        public ICommand NextImageCommand
        {
            get
            {
                if (nextImageCommand == null)
                {
                    nextImageCommand = new RelayCommand(
                        () => { CurrentImage = Pictures[++ImageIndex]; },
                        () => { return ImageIndex < Pictures.Count - 1; });
                }
                return nextImageCommand;
            }
        }

        private ICommand previousImageCommand;
        public ICommand PreviousImageCommand
        {
            get
            {
                if (previousImageCommand == null)
                {
                    previousImageCommand = new RelayCommand(
                        () => { CurrentImage = Pictures[--ImageIndex]; },
                        () => { return ImageIndex > 0; });
                }
                return previousImageCommand;
            }
        }

        private ICommand selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (selectImageCommand == null)
                {
                    selectImageCommand = new RelayCommand(
                        () =>
                        {
                            var dialog = new Microsoft.Win32.OpenFileDialog()
                            {
                                DefaultExt = "*.jpg",
                                InitialDirectory = "Images",
                                Filter = "图片文件(*.jpg,*.jpeg)|*.jpg;*.jpeg",
                                Multiselect = true
                            };
                            var result = dialog.ShowDialog();
                            if (result == true)
                            {
                                Pictures.Clear();
                                string[] files = dialog.FileNames;
                                foreach (var file in files)
                                {
                                    using (var fs = File.OpenRead(file))
                                    {
                                        var bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmap.DecodePixelWidth = 800;
                                        bitmap.StreamSource = fs;
                                        bitmap.EndInit();
                                        Pictures.Add(bitmap);
                                    }
                                }
                                ImageIndex = 0;
                                Copy.DefaultPictureIndex = ImageIndex;
                            }
                        });
                }
                return selectImageCommand;
            }
        }

        private ICommand setDefaultImageCommand;
        public ICommand SetDefaultImageCommand
        {
            get
            {
                if (setDefaultImageCommand == null)
                {
                    setDefaultImageCommand = new RelayCommand(
                        () => { Copy.DefaultPictureIndex = ImageIndex; },
                        () => { return CurrentImage != null && ImageIndex != Copy.DefaultPictureIndex; });
                }
                return setDefaultImageCommand;
            }
        }
        #endregion
        #endregion

        #region Combo Sources
        public IEnumerable<Age> RootAges { get => Repository<Age>.Get().Where(p => p.Parent == null); }
        public IEnumerable<Category> Categories { get => Repository<Category>.Get(); }
        public IEnumerable<CollectedTimeRange> CollectedTimeRanges { get => Repository<CollectedTimeRange>.Get(); }
        public IEnumerable<DisabilityLevel> DisabilityLevels { get => Repository<DisabilityLevel>.Get(); }
        public IEnumerable<Grain> RootGrains { get => Repository<Grain>.Get().Where(p => p.Parent == null); }
        public IEnumerable<Level> Levels { get => Repository<Level>.Get(); }
        public IEnumerable<RelicIdType> RelicIdTypes { get => Repository<RelicIdType>.Get(); }
        public IEnumerable<Source> Sources { get => Repository<Source>.Get(); }
        public IEnumerable<StoringCondition> StoringConditions { get => Repository<StoringCondition>.Get(); }
        public IEnumerable<WeightRange> WeightRanges { get => Repository<WeightRange>.Get(); }
        public IEnumerable<Unit> SizeUnits { get => Repository<Unit>.Get().Where(x => x.Type == UnitTypes.Length); }
        public IEnumerable<Unit> WeightUnits { get => Repository<Unit>.Get().Where(x => x.Type == UnitTypes.Weight); }
        public IEnumerable<Unit> RelicUnits { get => Repository<Unit>.Get().Where(x => x.Type == UnitTypes.Relic); }
        #endregion

        #region Public methods
        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (!saved)
            {
                if (dirty)
                {
                    Dialog.ShowMessage("是否保存更改？",
                        string.Format("藏品 {0}", string.IsNullOrEmpty(Copy.Name) ? string.Empty : " -- " + Copy.Name),
                        MessageBoxButton.YesNoCancel, out MessageBoxResult result);
                    switch (result)
                    {
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            break;
                        case MessageBoxResult.Yes:
                            if (SaveCommand.CanExecute(null))
                            {
                                SaveCommand.Execute(null);
                            }
                            else
                            {
                                Dialog.ShowMessage("藏品信息录入不全，无法保存。",
                                    string.Format("藏品 {0}", string.IsNullOrEmpty(Copy.Name) ? string.Empty : " -- " + Copy.Name),
                                    MessageBoxButton.OK, out result);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
