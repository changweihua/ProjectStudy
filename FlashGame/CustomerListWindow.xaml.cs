using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlashGame
{
    /// <summary>
    /// CustomerListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerListWindow : Elysium.Controls.Window
    {
        public CustomerListWindow()
        {
            InitializeComponent();
            lstProducts.ItemsSource = (from f in System.IO.Directory.GetFiles(@"E:\changweihua\Images", "*.png")
                                       select new System.IO.FileInfo(f)).ToList();
        }

        private void lstView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)lstView.SelectedItem;
            lstProducts.View = (ViewBase)this.FindResource(selectedItem.Content);
        }
    }
}
