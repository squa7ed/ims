using IMS.Common;
using IMS.Entity;
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
    class RelicViewModel : BaseViewModel
    {
        private bool isSaved;
        private bool isDirty;
        #region ctor
        public RelicViewModel()
        {
            isSaved = false;
            isDirty = false;
        }
        #endregion

        #region Properties
        private Relic relic;
        public Relic Relic { get => relic; set { relic = value; NotifyPropertyChanged(); Copy = Relic.Clone() as Relic; } }

        private Relic copy;
        public Relic Copy
        {
            get => copy; set { copy = value; NotifyPropertyChanged(); Copy.PropertyChanged += (o, e) => { isDirty = true; }; }
        }

        private string currentImage;
        public string CurrentImage
        {
            get
            {
                if (currentImage == null)
                {
                    if (Copy.Pictures.Count > 0)
                    {
                        currentImage = Copy.Pictures[0];
                    }
                }
                return currentImage;
            }
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
                        p =>
                        {
                            if (Repository<Relic>.Get().Any(x => x.Id == Copy.Id))
                            {
                                Repository<Relic>.Update(Copy);
                            }
                            else
                            {
                                Repository<Relic>.Add(Copy);
                            }
                            isSaved = true;
                            ViewManager.Backward();
                        },
                        p =>
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
                            return !hasDuplicate && (Copy.RootAge != null || Copy.SecondaryAge != null || Copy.ThirdAge != null || Copy.FourthAge != null) &&
                            Copy.Amount > 0 &&
                            Copy.Category != null &&
                            Copy.CollectedTimeRange != null &&
                            Copy.DisabilityLevel != null &&
                            (Copy.RootGrain != null || Copy.SecondaryGrain != null || Copy.ThirdGrain != null) &&
                            Copy.Level != null &&
                            Copy.Name != null &&
                            Copy.RelicId != null &&
                            Copy.IdType != null &&
                            Copy.Source != null &&
                            Copy.StoringCondition != null &&
                            Copy.WeightRange != null;
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
                        p =>
                        {
                            ViewManager.Backward();
                        });
                }
                return cancelCommand;
            }
        }

        private ICommand zoomInCommand;
        public ICommand ZoomInCommand
        {
            get
            {
                if (zoomInCommand == null)
                {
                    zoomInCommand = new RelayCommand(
                        p =>
                        {
                            if (p != null)
                            {
                            }
                        });
                }
                return zoomInCommand; ;
            }
        }
        private ICommand zoomOutCommand;
        public ICommand ZoomOutCommand
        {
            get
            {
                if (zoomOutCommand == null)
                {
                    zoomOutCommand = new RelayCommand(
                    p =>
                    {
                        if (p != null)
                        {
                        }
                    });
                }
                return zoomOutCommand;
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
                        p => { CurrentImage = Copy.Pictures[Copy.Pictures.IndexOf(CurrentImage) + 1]; },
                        p => { return Copy.Pictures.IndexOf(CurrentImage) < Copy.Pictures.Count - 1; });
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
                        p => { CurrentImage = Copy.Pictures[Copy.Pictures.IndexOf(CurrentImage) - 1]; },
                        p => { return Copy.Pictures.IndexOf(CurrentImage) > 0; });
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
                        o =>
                        {
                            var dialog = new Microsoft.Win32.OpenFileDialog()
                            {
                                DefaultExt = "*.jpg",
                                InitialDirectory = "Images",
                                Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                                Multiselect = true
                            };
                            var result = dialog.ShowDialog();
                            if (result == true)
                            {
                                string[] files = dialog.FileNames;
                                Copy.Pictures.Clear();
                                foreach (var item in files)
                                {
                                    Copy.Pictures.Add(item);
                                }
                                CurrentImage = Copy.Pictures[0];
                            }
                        });
                }
                return selectImageCommand;
            }
        }
        #endregion
        #endregion

        #region Combo Sources
        public IEnumerable<Age> RootAges { get => Repository<Age>.Get().Where(p => p.Parent == null); }
        public IEnumerable<Category> Categories { get => Repository<Category>.Get(); }
        public IEnumerable<CollectedTimeRange> CollectedTimeRanges { get => Repository<CollectedTimeRange>.Get(); }
        public IEnumerable<DisabilityCondition> DisabilityConditions { get => Repository<DisabilityCondition>.Get(); }
        public IEnumerable<DisabilityLevel> DisabilityLevels { get => Repository<DisabilityLevel>.Get(); }
        public IEnumerable<Grain> RootGrains { get => Repository<Grain>.Get().Where(p => p.Parent == null); }
        public IEnumerable<Level> Levels { get => Repository<Level>.Get(); }
        public IEnumerable<RelicIdType> RelicIdTypes { get => Repository<RelicIdType>.Get(); }
        public IEnumerable<Source> Sources { get => Repository<Source>.Get(); }
        public IEnumerable<StoringCondition> StoringConditions { get => Repository<StoringCondition>.Get(); }
        public IEnumerable<WeightRange> WeightRanges { get => Repository<WeightRange>.Get(); }
        #endregion

        #region Public methods
        public override void Dispose(object sender, CancelEventArgs e)
        {
            if (!isSaved)
            {
                if (isDirty)
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
