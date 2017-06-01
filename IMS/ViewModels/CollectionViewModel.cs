using IMS.Common;
using IMS.Entity;
using IMS.Common.ViewModels;
using IMS.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System;
using IMS.Common.Views;
using System.Windows;
using System.Collections;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading;

namespace IMS.ViewModels
{
    public partial class CollectionViewModel
    {
        private IList<Relic> selectedRelics = new List<Relic>();

        public CollectionViewModel() : base()
        {
            foreach (var relic in Repository<Relic>.Get())
            {
                relic.SelectionChanged += new SelectionChangedEventHandler(OnRelicSelectionChanged);
            }
        }
        #region Commands

        private ICommand importCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (importCommand == null)
                {
                    importCommand = new RelayCommand(
                      () =>
                      {
                          string standardExcel = "导入基于标准模板的Excel文件";
                          string selfAdjustingExcel = "导入自调整表头的Excel文件";
                          string sqlFile = "导入SQL数据库文件";
                          string[] selections = { standardExcel, selfAdjustingExcel, sqlFile };
                          Dialog.ShowSelection(selections, "演示版支持导入基于标准模板的Excel文件", out string result);
                          if (result != default(string))
                          {
                              var dlg = new Microsoft.Win32.OpenFileDialog()
                              {
                                  Filter = "Excel文件(*.xlsx)|*.xlsx",
                                  CheckPathExists = true,
                                  Multiselect = false,
                                  InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Resources")
                              };
                              var b = dlg.ShowDialog(Application.Current.MainWindow);
                              if (b.HasValue && b.Value)
                              {
                                  string file = dlg.FileName;
                                  var vm = new RelicImportViewModel() { FilePath = file };
                                  ViewManager.Show(new RelicImportView() { DataContext = vm });
                                  vm.ImportCommand.Execute(null);
                              }
                          }
                      });
                }
                return importCommand;
            }
        }

        //TODO Export
        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                if (exportCommand == null)
                {
                    exportCommand = new RelayCommand(
                      async () =>
                      {
                          string exportToExcel = "导出为Excel文件";
                          string exportToPdf = "导出为DPF文件";
                          string exportToXps = "导出为XPS文件";
                          string[] selections = { exportToPdf, exportToExcel, exportToXps };
                          Dialog.ShowSelection(selections, $"演示版支持导出为PDF文件,将导出{selectedRelics.Count}条数据", out string selection);
                          if (selection != default(string))
                          {
                              string exportDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                              var dlg = new System.Windows.Forms.FolderBrowserDialog();
                              var result = dlg.ShowDialog();
                              switch (result)
                              {
                                  case System.Windows.Forms.DialogResult.OK:
                                  case System.Windows.Forms.DialogResult.Yes:
                                      ViewManager.Loading();
                                      string fileName = string.Format("导出藏品-{0}.pdf", DateTime.Now.ToString("yyyyMMdd"));
                                      string exportFile = Path.Combine(exportDir, fileName);
                                      await Task.Run(() =>
                                      {
                                          ExportPdfToFile(selectedRelics, exportFile);
                                      });
                                      ViewManager.LoadingFinished();
                                      break;
                              }
                          }
                      },
                      () => { return selectedRelics.Count > 0; });
                }
                return exportCommand;
            }
        }

        //TODO Repair
        public ICommand RepairCommand { get; set; }

        //TODO Replicate
        public ICommand ReplicateCommand { get; set; }

        private ICommand selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                if (selectAllCommand == null)
                {
                    selectAllCommand = new RelayCommand(
                        () =>
                        {
                            foreach (var relic in Entities)
                            {
                                relic.IsSelected = true;
                            }
                        });
                }
                return selectAllCommand;
            }
        }

        private ICommand unselectAllCommand;
        public ICommand UnselectAllCommand
        {
            get
            {
                if (unselectAllCommand == null)
                {
                    unselectAllCommand = new RelayCommand(
                        () =>
                        {
                            foreach (var relic in Entities)
                            {
                                relic.IsSelected = false;
                            }
                        },
                        () => { return selectedRelics.Count > 0; });
                }
                return unselectAllCommand;
            }
        }
        #endregion

        private void OnRelicSelectionChanged(ISelectable sender)
        {
            if (sender.IsSelected)
            {
                selectedRelics.Add(sender as Relic);
            }
            else
            {
                selectedRelics.Remove(sender as Relic);
            }
        }

        private void ExportPdfToFile(IList<Relic> list, string file)
        {
            Stream fs = null;
            try
            {
                fs = File.Create(file);
                Document doc = new Document(PageSize.A4.Rotate(), 18, 18, 36, 18);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bf, 10, Font.NORMAL);
                Font fontT = new Font(bf, 13, Font.BOLD);
                Font fontB = new Font(bf, 10, Font.BOLD);

                Font fontI = new Font(bf, 10, Font.ITALIC, Color.DARK_GRAY);

                doc.Add(new Paragraph("藏品目录", fontT) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph(12, DateTime.Now.ToLongDateString(), fontI) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new Paragraph(12, string.IsNullOrWhiteSpace(FilterText) ? "所有藏品" : "筛选：" + FilterText, fontI) { Alignment = Element.ALIGN_CENTER });

                PdfPTable table = new PdfPTable(new float[] { 10, 10, 35, 10, 10, 10, 10, 10, 10, 13 })
                {
                    SpacingBefore = 12
                };
                Thickness border = new Thickness(0, 0, 0, 1);
                Thickness padding = new Thickness(5);
                int va = Element.ALIGN_MIDDLE;
                int ha = Element.ALIGN_CENTER;
                string[] titles = { "编号类型", "编号", "名称", "文物级别", "文物类别", "年代", "质地", "来源", "完残程度", "图片" };
                AddTitleRow(table, titles, fontB);
                foreach (var relic in list)
                {
                    table.AddCell(CreateTextCell(relic.IdType.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.RelicId, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Level.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Category.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Age.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Grain.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.Source.Name, font, va, ha, padding, border));
                    table.AddCell(CreateTextCell(relic.DisabilityLevel.Name, font, va, ha, padding, border));
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(relic.Pictures[relic.DefaultPictureIndex]);
                    img.ScaleAbsoluteWidth(128);
                    table.AddCell(new PdfPCell()
                    {
                        Image = img,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BorderWidthBottom = (float)border.Bottom,
                        BorderWidthTop = (float)border.Top,
                        BorderWidthRight = (float)border.Right,
                        BorderWidthLeft = (float)border.Left,
                        PaddingLeft = (float)padding.Left,
                        PaddingTop = (float)padding.Top,
                        PaddingRight = (float)padding.Right,
                        PaddingBottom = (float)padding.Bottom
                    });
                }
                doc.Add(table);
                doc.Close();
                Application.Current.Dispatcher.BeginInvoke((Action)delegate { Dialog.ShowMessage(string.Format("成功导出{0}条数据到文件{1}", list.Count, file), "藏品导出成功！", MessageBoxButton.OK, out MessageBoxResult tmp); });
            }
            catch (Exception)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate { Dialog.ShowMessage("无法导出到指定的文件夹中，请确认具有相关权限、文件未被其他进程占用。", "藏品导出失败!", MessageBoxButton.OK, out MessageBoxResult tmp); });
            }
            finally { if (fs != null) fs.Dispose(); }
        }

        private void AddTitleRow(PdfPTable table, string[] titles, Font font)
        {
            foreach (var item in titles)
            {
                table.AddCell(CreateTextCell(item, font, Element.ALIGN_MIDDLE, Element.ALIGN_CENTER, new Thickness(5), new Thickness(0, 2, 0, 1)));
            }
        }

        private PdfPCell CreateTextCell(string content, Font font, int verticalAlignment, int horizontalAlignment, Thickness padding, Thickness borderThickness)
        {
            return new PdfPCell(new Phrase(content, font))
            {
                VerticalAlignment = verticalAlignment,
                HorizontalAlignment = horizontalAlignment,
                BorderWidthTop = (float)borderThickness.Top,
                BorderWidthBottom = (float)borderThickness.Bottom,
                BorderWidthLeft = (float)borderThickness.Left,
                BorderWidthRight = (float)borderThickness.Right,
                PaddingTop = (float)padding.Top,
                PaddingBottom = (float)padding.Bottom,
                PaddingRight = (float)padding.Right,
                PaddingLeft = (float)padding.Left,
            };
        }
    }

    public partial class CollectionViewModel : CollectionViewModelBase<Relic>
    {
        private Relic entity;
        public override Relic Entity
        {
            get => entity;
            set
            {
                if (entity != null)
                {
                    entity.IsSelected = false;
                }
                if (value != null)
                {
                    entity = value;
                    entity.IsSelected = true;
                    NotifyPropertyChanged();
                }
            }
        }

        private ICommand addCommand;
        public override ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new RelicView() { DataContext = new RelicViewModel() { Relic = new Relic() } });
                        });
                }
                return addCommand;
            }
            set { addCommand = value; }
        }

        private ICommand editCommand;
        public override ICommand EditCommand
        {
            get
            {
                if (editCommand == null)
                {
                    editCommand = new RelayCommand(
                        () =>
                        {
                            ViewManager.Show(new RelicView() { DataContext = new RelicViewModel() { Relic = Entity } });
                        },
                        () => { return Entity != null; });
                }
                return editCommand;
            }
            set { editCommand = value; }
        }


        public override void Dispose(object sender, CancelEventArgs e)
        {
        }

        protected async override void OnRepositoryChanged(Relic entity)
        {
            ViewManager.Loading();
            await Task.Run(() =>
            {
                if (entity != null)
                {
                    entity.SelectionChanged += new SelectionChangedEventHandler(OnRelicSelectionChanged);
                }
                ResetFilterCommand.Execute(null);
            });
            ViewManager.LoadingFinished();
        }

    }

    public partial class CollectionViewModel : IFilterable<Relic>
    {
        private Dictionary<string, HashSet<IFilter<Relic>>> currentFilters = new Dictionary<string, HashSet<IFilter<Relic>>>();

        private IEnumerable<IFilter<Relic>> filterList;
        public IEnumerable<IFilter<Relic>> FilterList
        {
            get
            {
                if (filterList == null)
                {
                    filterList = CreateFilters();
                }
                return filterList;
            }
            set { filterList = value; NotifyPropertyChanged(); }
        }

        private ICommand resetCommand;
        public ICommand ResetFilterCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new RelayCommand(
                       async () =>
                       {
                           ViewManager.Loading();
                           if (currentFilters.Count > 0)
                           {
                               await Task.Run(() =>
                               {
                                   currentFilters.Clear();
                                   var list = CreateFilters();
                                   Application.Current.Dispatcher.BeginInvoke((Action)delegate { FilterList = list; });
                               });
                           }
                           Filter();
                           ViewManager.LoadingFinished();
                       });
                }
                return resetCommand;
            }
        }

        private string filterText;
        public string FilterText { get => filterText; set { filterText = value; NotifyPropertyChanged(); } }

        private IFilter<Relic> currentFilter;
        public IFilter<Relic> CurrentFilter
        {
            get => currentFilter;
            set
            {
                if (currentFilter != null && value != null && currentFilter.Title == value.Title)
                {
                    currentFilter.IsSelected = false;
                }
                currentFilter = value;
                if (value != null)
                {
                    currentFilter.IsSelected = true;
                }
                NotifyPropertyChanged();
            }
        }

        public void Filter()
        {
            var mEntities = new List<Relic>(Repository<Relic>.Get());
            string mFilterText = string.Empty;
            int cnt = 0;
            foreach (var filters in currentFilters.Values)
            {
                var list = new HashSet<Relic>();
                foreach (var filter in filters)
                {
                    list.UnionWith(mEntities.Where(filter.FilterPredicate));
                    mFilterText += filter.Name + "; ";
                    cnt++;
                }
                mEntities.Intersect(list);
            }
            if (mFilterText.EndsWith("; "))
            {
                mFilterText = mFilterText.Remove(mFilterText.Length - 2);
            }
            Application.Current.Dispatcher.BeginInvoke((Action)delegate { Entities = mEntities; FilterText = mFilterText; });
        }

        private IEnumerable<IFilter<Relic>> CreateFilters()
        {
            var list = new HashSet<IFilter<Relic>>() { new Category(), new Level(), new Grain(), new Age(), new Source() };
            foreach (var filters in list)
            {
                foreach (var filter in filters.Filters)
                {
                    filter.SelectionChanged += async (o) =>
                    {
                        ViewManager.Loading();
                        await Task.Run(() =>
                        {
                            if (UnselectAllCommand.CanExecute(null))
                            {
                                UnselectAllCommand.Execute(null);
                            }
                            if (filter.IsSelected)
                            {
                                if (!currentFilters.ContainsKey(filter.Title))
                                {
                                    currentFilters.Add(filter.Title, new HashSet<IFilter<Relic>>());
                                }
                                currentFilters[filter.Title].Add(filter);
                            }
                            else
                            {
                                if (CurrentFilter == filter)
                                {
                                    CurrentFilter = null;
                                }
                                if (currentFilters.ContainsKey(filter.Title))
                                {
                                    currentFilters[filter.Title].Remove(filter);
                                    if (currentFilters[filter.Title].Count == 0)
                                    {
                                        currentFilters.Remove(filter.Title);
                                    }
                                }
                            }
                            Filter();
                        });
                        ViewManager.LoadingFinished();
                    };
                }
            }
            return list;
        }
    }
}
