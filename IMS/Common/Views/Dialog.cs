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

namespace IMS.Common.Views
{
    public class Dialog : Window
    {
        private const string OK = "确认";
        private const string Cancel = "取消";
        private const string Yes = "是";
        private const string No = "否";

        private static Dialog instance;

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
            Loaded += (o, e) =>
            {
                var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
                BeginAnimation(OpacityProperty, anim);
            };
            Closing += (o, e) =>
            {
                var anim = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
                BeginAnimation(OpacityProperty, anim);
            };
        }

        public static void Show(object uc)
        {
            instance = new Dialog() { Content = uc }; ;
            instance.ShowDialog();
        }

        public static void Change(object uc)
        {
            if (instance != null)
            {
                instance.Content = uc;
            }
            else
            {
                instance = new Dialog() { Content = uc };
            }
        }

        public static void Dispose()
        {
            if (instance != null)
            {
                instance.Close();
            }
        }

        public static void ShowMessage(string message, string title, MessageBoxButton buttons, out MessageBoxResult result)
        {
            MessageBoxResult Result = MessageBoxResult.None;
            Grid grid = null;
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    grid = CreateGrid(message, title, new string[] { OK });
                    break;
                case MessageBoxButton.OKCancel:
                    grid = CreateGrid(message, title, new string[] { OK, Cancel });
                    break;
                case MessageBoxButton.YesNoCancel:
                    grid = CreateGrid(message, title, new string[] { Yes, No, Cancel });
                    break;
                case MessageBoxButton.YesNo:
                    grid = CreateGrid(message, title, new string[] { Yes, No });
                    break;
                default:
                    break;
            }
            var dialog = new Dialog() { Content = grid };
            foreach (var btn in grid.Children)
            {
                if (btn is Button)
                {
                    (btn as Button).Click += (o, e) =>
                    {
                        switch ((btn as Button).Content)
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

        private static Grid CreateGrid(string message, string title, string[] contents)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < contents.Length; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                var btn = new Button() { Content = contents[i], Style = Application.Current.TryFindResource("DialogPanelButton") as Style };
                Grid.SetRow(btn, 3);
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
            Grid.SetRow(ti, 1);
            Grid.SetColumn(ti, 1);
            Grid.SetColumnSpan(ti, contents.Length);
            Grid.SetRow(msg, 2);
            Grid.SetColumn(msg, 1);
            Grid.SetColumnSpan(msg, contents.Length);
            grid.Children.Add(ti);
            grid.Children.Add(msg);
            return grid;
        }
    }
}
