using IMS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Windows.Input;
using IMS.Common;
using System.Threading;
using IMS.Common.Views;
using IMS.Entity;
using System.Windows;
using IMS.Views;
using System.IO;
using Excel;
using System.Windows.Media.Imaging;

namespace IMS.ViewModels
{
    public class RelicImportViewModel : ViewModelBase
    {
        public string FilePath { get; set; }

        public string PicturePath { get; set; }

        private DataTable relics;
        public DataTable Relics
        {
            get => relics;
            set { relics = value; NotifyPropertyChanged(); }
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

        private ICommand confirmCommand;
        public ICommand ConfirmCommand
        {
            get
            {
                if (confirmCommand == null)
                {
                    confirmCommand = new RelayCommand(
                        async () =>
                        {
                            if (Relics != null && Relics.Rows.Count > 0)
                            {
                                ViewManager.Loading();
                                await Task.Run((Action)ImportData);
                                ViewManager.Backward();
                                ViewManager.LoadingFinished();
                            }
                            else
                            {
                                Dialog.ShowMessage("所选文件不是标准模板文件，或文件为空。", "无法导入所选文件", MessageBoxButton.OK, out MessageBoxResult tmp);
                                ViewManager.Backward();
                            }
                        });
                }
                return confirmCommand;
            }
        }

        private ICommand setPicturePathCommand;
        public ICommand SetPicturePathCommand
        {
            get
            {
                if (setPicturePathCommand == null)
                {
                    setPicturePathCommand = new RelayCommand(
                        () =>
                        {
                            var dlg = new System.Windows.Forms.FolderBrowserDialog();
                            var result = dlg.ShowDialog();
                            switch (result)
                            {
                                case System.Windows.Forms.DialogResult.OK:
                                case System.Windows.Forms.DialogResult.Yes:
                                    PicturePath = dlg.SelectedPath;
                                    break;
                            }
                        });
                }
                return setPicturePathCommand;
            }
        }

        private void ImportData()
        {
            int imported = 0;
            int exist = 0;
            int failed = 0;
            int row = 0;
            foreach (DataRow dr in Relics.Rows)
            {
                row++;
                try
                {
                    var relic = new Relic()
                    {
                        Category = Repository<Category>.Get().FirstOrDefault(x => x.Name == dr["*文物类别"].ToString()),
                        Name = dr["*名    称"].ToString(),
                        CollectedTimeRange = Repository<CollectedTimeRange>.Get().FirstOrDefault(x => x.Name == dr["*入藏时间范围"].ToString()),
                        CollectedYearOfTime = dr["入藏年度"].ToString(),
                        DisabilityCondition = dr["完残状况"].ToString(),
                        DisabilityLevel = Repository<DisabilityLevel>.Get().FirstOrDefault(x => x.Name == dr["*完残程度"].ToString()),
                        RootAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代1"].ToString()),
                        SecondaryAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代2"].ToString()),
                        ThirdAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代3"].ToString()),
                        FourthAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代4"].ToString()),
                        IdType = Repository<RelicIdType>.Get().FirstOrDefault(x => x.Name == dr["*编号类型"].ToString()),
                        Level = Repository<Level>.Get().FirstOrDefault(x => x.Name == dr["*文物级别"].ToString()),
                        OriginalName = dr["原    名"].ToString(),
                        RelicId = dr["*编号"].ToString(),
                        RootGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地类别1"].ToString()),
                        SecondaryGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地类别2"].ToString()),
                        ThirdGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地"].ToString()),
                        SizeUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Name == "cm"),
                        Source = Repository<Source>.Get().FirstOrDefault(x => x.Name == dr["*文物来源"].ToString()),
                        SpecificAge = dr["具体年代"].ToString(),
                        SpecificSize = dr["具体尺寸"].ToString(),
                        StoringCondition = Repository<StoringCondition>.Get().FirstOrDefault(x => x.Name == dr["保存状态"].ToString()),
                        WeightRange = Repository<WeightRange>.Get().FirstOrDefault(x => x.Name == dr["*质量范围"].ToString()),
                        WeightUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Name == dr["单位"].ToString())
                    };
                    //var relic = new Relic();
                    //relic.Category = Repository<Category>.Get().FirstOrDefault(x => x.Name == dr["*文物类别"].ToString());
                    //relic.Name = dr["*名    称"].ToString();
                    //relic.CollectedTimeRange = Repository<CollectedTimeRange>.Get().FirstOrDefault(x => x.Name == dr["*入藏时间范围"].ToString());
                    //relic.CollectedYearOfTime = dr["入藏年度"].ToString();
                    //relic.DisabilityCondition = dr["完残状况"].ToString();
                    //relic.DisabilityLevel = Repository<DisabilityLevel>.Get().FirstOrDefault(x => x.Name == dr["*完残程度"].ToString());
                    //relic.RootAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代1"].ToString());
                    //relic.SecondaryAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代2"].ToString());
                    //relic.ThirdAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代3"].ToString());
                    //relic.FourthAge = Repository<Age>.Get().FirstOrDefault(x => x.Name == dr["*年代4"].ToString());
                    //relic.IdType = Repository<RelicIdType>.Get().FirstOrDefault(x => x.Name == dr["*编号类型"].ToString());
                    //relic.Level = Repository<Level>.Get().FirstOrDefault(x => x.Name == dr["*文物级别"].ToString());
                    //relic.OriginalName = dr["原    名"].ToString();
                    //relic.RelicId = dr["*编号"].ToString();
                    //relic.RootGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地类别1"].ToString());
                    //relic.SecondaryGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地类别2"].ToString());
                    //relic.ThirdGrain = Repository<Grain>.Get().FirstOrDefault(x => x.Name == dr["*质地"].ToString());
                    //relic.SizeUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Name == "cm");
                    //relic.Source = Repository<Source>.Get().FirstOrDefault(x => x.Name == dr["*文物来源"].ToString());
                    //relic.SpecificAge = dr["具体年代"].ToString();
                    //relic.SpecificSize = dr["具体尺寸"].ToString();
                    //relic.StoringCondition = Repository<StoringCondition>.Get().FirstOrDefault(x => x.Name == dr["保存状态"].ToString());
                    //relic.WeightRange = Repository<WeightRange>.Get().FirstOrDefault(x => x.Name == dr["*质量范围"].ToString());
                    //relic.WeightUnit = Repository<Unit>.Get().FirstOrDefault(x => x.Name == dr["单位"].ToString());
                    double.TryParse(dr["通高"].ToString(), out double height);
                    relic.Height = height;
                    double.TryParse(dr["通长"].ToString(), out double length);
                    relic.Length = length;
                    int.TryParse(dr["*实际数量"].ToString(), out int totalAmount);
                    relic.TotalAmount = totalAmount;
                    double.TryParse(dr["具体质量"].ToString(), out double weight);
                    relic.Weight = weight;
                    double.TryParse(dr["通宽"].ToString(), out double width);
                    relic.Width = width;
                    var authorName = dr["著者"].ToString();
                    if (!string.IsNullOrWhiteSpace(authorName))
                    {
                        Author author = Repository<Author>.Get().FirstOrDefault(x => string.Equals(authorName, x.Name, StringComparison.CurrentCultureIgnoreCase));
                        if (author == null)
                        {
                            author = new Author() { Name = authorName };
                            Repository<Author>.Add(author);
                        }
                        relic.Author = author;
                    }
                    if (relic.CanSave())
                    {
                        if (!Repository<Relic>.Get().Any(x => x.RelicId == relic.RelicId))
                        {
                            Repository<Relic>.Add(relic);
                            imported++;
                        }
                        else
                        {
                            exist++;
                        }
                    }
                    else
                    {
                        failed++;
                    }
                }
                catch (Exception e)
                {
                    Log.Warning(e.Message);
                    Log.Warning("Error while importing data from {0} at row {1}.", FilePath, row);
                }
            }
            if (PicturePath != null)
            {
                ImportPictures();
            }
            Repository<Relic>.SaveChanges();
            Application.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                Dialog.ShowMessage(string.Format("藏品总数: {0}\n重复条数: {1}\n无法导入: {2}\n成功导入: {3}", Relics.Rows.Count, exist, failed, imported),
                    string.Format("导入文件 {0}。", FilePath),
                    MessageBoxButton.OK, out MessageBoxResult result);
            });
        }

        private void ImportPictures()
        {
            var files = new Dictionary<string, HashSet<string>>();
            GetFiles(files);
            foreach (var relicId in files.Keys)
            {
                ImportPicture(relicId, files[relicId]);
            }
        }

        private void ImportPicture(string relicId, HashSet<string> pictures)
        {
            var relic = Repository<Relic>.Get().FirstOrDefault(x => x.RelicId == relicId);
            if (relic != null)
            {
                if (relic.Pictures == null)
                {
                    relic.Pictures = new List<byte[]>(pictures.Count);
                    foreach (var pic in pictures)
                    {
                        using (var fs = File.OpenRead(pic))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.DecodePixelWidth = 800;
                            bitmap.StreamSource = fs;
                            bitmap.EndInit();
                            var encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmap));
                            using (var ms = new MemoryStream())
                            {
                                encoder.Save(ms);
                                relic.Pictures.Add(ms.ToArray());
                            }
                        }
                    }
                }
                Repository<Relic>.Update(relic);
            }
        }

        private void GetFiles(Dictionary<string, HashSet<string>> files)
        {
            foreach (var dir in Directory.EnumerateDirectories(PicturePath))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                if (!files.ContainsKey(di.Name))
                {
                    files.Add(di.Name, new HashSet<string>());
                }
                foreach (var item in Directory.EnumerateFiles(dir, "*.jpg"))
                {
                    files[di.Name].Add(item);
                }
            }
        }

        private ICommand importCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (importCommand == null)
                {
                    importCommand = new RelayCommand(
                      async () =>
                        {
                            ViewManager.Loading();
                            Relics = await Task.Run((Func<DataTable>)GetAll);
                            ViewManager.LoadingFinished();
                        });
                }
                return importCommand;
            }
        }

        private DataTable GetAll()
        {
            FileStream fs = null;
            IExcelDataReader reader = null;

            try
            {
                fs = File.Open(FilePath, FileMode.Open, FileAccess.Read);
                reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                if (reader != null)
                {
                    reader.IsFirstRowAsColumnNames = true;
                    var dataSet = reader.AsDataSet();
                    if (dataSet?.Tables?.Count > 0)
                    {
                        var dt = dataSet.Tables[0];
                        return dt;
                    }
                }
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    Dialog.ShowMessage(
                        "文件格式错误，请使用标准模板文件导入。",
                        string.Format("无法导入文件 {0}。", FilePath),
                        MessageBoxButton.OK, out MessageBoxResult result);
                    ViewManager.Backward();
                });
                return null;
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    Dialog.ShowMessage(
                        e.Message,
                        string.Format("无法导入文件 {0}。", FilePath),
                        MessageBoxButton.OK, out MessageBoxResult result);
                });
                ViewManager.Backward();
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
        }

        public override void Dispose(object sender, CancelEventArgs e)
        {

        }
    }
}
