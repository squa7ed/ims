using IMS.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using IMS.Common.Views;
using System.Windows;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Controls;

namespace IMS.ViewModels
{
    public partial class RelicImportViewModel : BaseViewModel
    {
        private string file;

        private SelectionItem selection;

        public RelicImportViewModel()
        {
            Selections = new List<SelectionItem>()
            {
                new SelectionItem()
                {
                    ImportType = ImportType.StandardExcel,
                    FileFilter ="Excel文件（*.xls,*.xlsx）|*.xls;*.xlsx",
                    Text ="导入基于标准模板的Excel文件"
                },
                new SelectionItem()
                {
                    ImportType = ImportType.SelfAdjustingExcel,
                    FileFilter ="Excel文件（*.xls,*.xlsx）|*.xls;*.xlsx",
                    Text ="导入用户对应表头的的Excel文件"
                },
                new SelectionItem()
                {
                    ImportType = ImportType.SqlFile,
                    FileFilter ="Sql文件|*.*",
                    Text ="导入SQL数据库文件"
                }
            };
            Selections.ForEach(item => item.PropertyChanged += OnItemSelected);
            ShowSelection();
        }
        public List<SelectionItem> Selections { get; set; }

        private Visibility selectionVisibility;
        public Visibility SelectionVisibility { get => selectionVisibility; set { selectionVisibility = value; NotifyPropertyChanged(); } }

        private Visibility importVisibility;
        public Visibility ImportVisibility { get => importVisibility; set { importVisibility = value; NotifyPropertyChanged(); } }

        private ICommand confirmSelectionCommand;
        public ICommand ConfirmSelectionCommand
        {
            get
            {
                if (confirmSelectionCommand == null)
                {
                    confirmSelectionCommand = new RelayCommand(
                        async p =>
                        {
                            var dialog = new Microsoft.Win32.OpenFileDialog()
                            {
                                CheckPathExists = true,
                                Filter = selection.FileFilter
                            };
                            var hasSelection = dialog.ShowDialog();
                            if (hasSelection.Value)
                            {
                                file = dialog.FileName;
                                Dialog.ShowMessage(string.Format("确认导入{0}？", file), "藏品导入", MessageBoxButton.YesNo, out MessageBoxResult result);
                                switch (selection.ImportType)
                                {
                                    case ImportType.SqlFile:
                                    case ImportType.SelfAdjustingExcel:
                                        Dialog.ShowMessage("基础版不包含导入数据库文件、自适应excel等选项。", "基础版软件", MessageBoxButton.OK, out MessageBoxResult tmp);
                                        break;
                                    case ImportType.StandardExcel:
                                        var uc = Dialog.CurrentContent;
                                        Dialog.Dispose();
                                        ShowImport();
                                        ViewManager.Loading();
                                        ImportingData = await Task.Run(delegate
                                        {
                                            string connString = string.Format(" Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source ={0};Extended Properties=Excel 8.0", file);
                                            var importingData = new DataTable();
                                            using (var conn = new OleDbConnection(connString))
                                            {
                                                string sql = @"SELECT * FROM [Sheet1$]";
                                                using (var adapter = new OleDbDataAdapter(sql, conn))
                                                {
                                                    adapter.Fill(importingData);
                                                }
                                            }
                                            return importingData;
                                        });
                                        ViewManager.Show(uc as UserControl);
                                        break;
                                }
                            }
                        },
                        p =>
                        {
                            return selection != null;
                        });
                }
                return confirmSelectionCommand;
            }
        }

        private ICommand cancelSelectionCommand;
        public ICommand CancelSelectionCommand
        {
            get
            {
                if (cancelSelectionCommand == null)
                {
                    cancelSelectionCommand = new RelayCommand(p => { Dialog.Dispose(); });
                }
                return cancelSelectionCommand;
            }
        }

        private void OnItemSelected(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as SelectionItem;
            if (item.IsSelected)
            {
                selection = item;
            }
        }

        private void ShowSelection()
        {
            SelectionVisibility = Visibility.Visible;
            ImportVisibility = Visibility.Collapsed;
        }

        private void ShowImport()
        {
            SelectionVisibility = Visibility.Collapsed;
            ImportVisibility = Visibility.Visible;
        }
    }

    public partial class RelicImportViewModel
    {
        private DataTable importingData;
        public DataTable ImportingData { get => importingData; set { importingData = value; NotifyPropertyChanged(); } }


        private ICommand confirmImportCommand;
        public ICommand ConfirmImportCommand
        {
            get
            {
                if (confirmImportCommand == null)
                {
                    confirmImportCommand = new RelayCommand(
                        p =>
                        {

                        },
                        p =>
                        {
                            return true;
                        });
                }
                return confirmImportCommand;
            }
        }

        private ICommand cancelImportCommand;
        public ICommand CancelImportCommand
        {
            get
            {
                if (cancelImportCommand == null)
                {
                    cancelImportCommand = new RelayCommand(
                        p =>
                        {
                            ViewManager.Backward();
                        });
                }
                return cancelImportCommand;
            }
        }

        public override void Dispose(object sender, CancelEventArgs e)
        {
            Dialog.ShowMessage("确认取消导入？", "藏品导入", MessageBoxButton.YesNoCancel, out MessageBoxResult result);
            switch (result)
            {
                case MessageBoxResult.Cancel:
                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    break;
            }
        }
    }

    public partial class RelicImportViewModel
    {
        public class SelectionItem : INotifyPropertyChanged
        {
            private bool isSelected;
            public bool IsSelected { get => isSelected; set { isSelected = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected")); } }

            public string Text { get; set; }

            public string FileFilter { get; set; }

            public ImportType ImportType { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public enum ImportType
        {
            StandardExcel,
            SelfAdjustingExcel,
            SqlFile
        }
    }
}
