using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IMS.Views;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace IMS.Common.Views
{
    public class Dialog : Window
    {
        private const string OK = "确认";
        private const string Cancel = "取消";
        private const string Yes = "是";
        private const string No = "否";

        private bool closingFinished;

        private Dialog()
        {
            Owner = Application.Current.MainWindow;
            Width = Application.Current.MainWindow.ActualWidth;
            SizeToContent = SizeToContent.Height;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            Background = Application.Current.TryFindResource("DialogPanelBackgroundBrush") as Brush;
            closingFinished = false;
            Loaded += (o, e) =>
            {
                var anim = new DoubleAnimation(-Height, (Owner.ActualHeight - Height) / 2, TimeSpan.FromMilliseconds(500));
                BeginAnimation(TopProperty, anim);
            };

            Closing += (o, e) =>
            {
                if (!closingFinished)
                {
                    e.Cancel = true;
                    var anim = new DoubleAnimation(Top, Owner.ActualHeight + Height, TimeSpan.FromMilliseconds(500));
                    anim.Completed += (o1, e1) => { closingFinished = true; Close(); };
                    BeginAnimation(TopProperty, anim);
                }
            };
        }

        public static void ShowMessage(string message, string title, MessageBoxButton buttons, out MessageBoxResult result)
        {
            MessageBoxResult Result = MessageBoxResult.None;
            Panel grid = null;
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    grid = CreateMessagePanel(message, title, new string[] { OK });
                    break;
                case MessageBoxButton.OKCancel:
                    grid = CreateMessagePanel(message, title, new string[] { OK, Cancel });
                    break;
                case MessageBoxButton.YesNoCancel:
                    grid = CreateMessagePanel(message, title, new string[] { Yes, No, Cancel });
                    break;
                case MessageBoxButton.YesNo:
                    grid = CreateMessagePanel(message, title, new string[] { Yes, No });
                    break;
                default:
                    break;
            }
            var dialog = new Dialog() { Content = grid };
            foreach (var item in grid.Children)
            {
                if (item is Button)
                {
                    (item as Button).Click += (o, e) =>
                    {
                        switch ((item as Button).Content)
                        {
                            case OK:
                                Result = MessageBoxResult.OK;
                                break;
                            case Cancel:
                                Result = MessageBoxResult.Cancel;
                                break;
                            case Yes:
                                Result = MessageBoxResult.Yes;
                                break;
                            case No:
                                Result = MessageBoxResult.No;
                                break;
                        }
                        dialog.Close();
                    };
                }
            }
            dialog.ShowDialog();
            result = Result;
        }

        public static void ShowSelection<T>(T[] selections, string title, out T selection)
        {
            T _selection = default(T);
            var panel = CreateSelectionPanel(selections, title);
            var dialog = new Dialog() { Content = panel };
            foreach (var item in panel.Children)
            {
                if (item is RadioButton)
                {
                    var rb = item as RadioButton;
                    if (rb.IsChecked.HasValue && rb.IsChecked.Value)
                    {
                        _selection = (T)rb.Content;
                    }
                    rb.Checked += (o, e) =>
                    {
                        _selection = (T)rb.Content;
                    };
                }
                if (item is Button)
                {
                    (item as Button).Click += (o, e) =>
                    {
                        switch ((item as Button).Content)
                        {
                            case OK:
                                break;
                            case Cancel:
                                _selection = default(T);
                                break;
                        }
                        dialog.Close();
                    };
                }
            }
            dialog.ShowDialog();
            selection = _selection;
        }

        private static Panel CreateSelectionPanel<T>(T[] selections, string title)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < selections.Length; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto, SharedSizeGroup = "Selection" });
                var radio = new RadioButton() { Content = selections[i], Style = Application.Current.TryFindResource("DialogPanelRadioButton") as Style, IsChecked = i == 0 };
                Grid.SetRow(radio, i + 1);
                Grid.SetColumn(radio, 1);
                Grid.SetColumnSpan(radio, 2);
                grid.Children.Add(radio);
            }
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            var okBtn = new Button() { Content = OK, Style = Application.Current.TryFindResource("DialogPanelButton") as Style };
            var cancelBtn = new Button() { Content = Cancel, Style = Application.Current.TryFindResource("DialogPanelButton") as Style };
            Grid.SetRow(okBtn, selections.Length + 1);
            Grid.SetColumn(okBtn, 1);
            Grid.SetRow(cancelBtn, selections.Length + 1);
            Grid.SetColumn(cancelBtn, 2);
            grid.Children.Add(okBtn);
            grid.Children.Add(cancelBtn);
            var ti = new TextBlock()
            {
                Text = title,
                Style = Application.Current.TryFindResource("DialogPanelTitle") as Style,
            };
            Grid.SetRow(ti, 0);
            Grid.SetColumn(ti, 1);
            Grid.SetColumnSpan(ti, 2);
            grid.Children.Add(ti);
            return grid;
        }

        private static Panel CreateMessagePanel(string message, string title, string[] btnContent)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < btnContent.Length; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                var btn = new Button() { Content = btnContent[i], Style = Application.Current.TryFindResource("DialogPanelButton") as Style };
                Grid.SetRow(btn, 2);
                Grid.SetColumn(btn, i + 1);
                grid.Children.Add(btn);
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            var ti = new TextBlock()
            {
                Text = title,
                Style = Application.Current.TryFindResource("DialogPanelTitle") as Style,
            };
            var msg = new TextBlock()
            {
                Text = message,
                Style = Application.Current.TryFindResource("DialogPanelText") as Style,
            };
            Grid.SetRow(ti, 0);
            Grid.SetColumn(ti, 1);
            Grid.SetColumnSpan(ti, btnContent.Length);
            Grid.SetRow(msg, 1);
            Grid.SetColumn(msg, 1);
            Grid.SetColumnSpan(msg, btnContent.Length);
            grid.Children.Add(ti);
            grid.Children.Add(msg);
            return grid;
        }
    }
}
