using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for ProductReceiptView.xaml
    /// </summary>
    public partial class ReceiptView : UserControl
    {
        public ReceiptView()
        {
            InitializeComponent();
        }

        private void ShowSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(this, "OnSelection", true);
        }

        private void ConfirmSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(this, "Normal", true);
        }

        private void CancelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(this, "Normal", true);
        }
    }
}
