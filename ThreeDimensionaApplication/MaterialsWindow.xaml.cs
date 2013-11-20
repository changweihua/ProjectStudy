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
using System.Windows.Media.Media3D;

namespace ThreeDimensionaApplication
{
    /// <summary>
    /// MaterialsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialsWindow : Window
    {
        public MaterialsWindow()
        {
            InitializeComponent();
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            materialsGroup.Children.Clear();
            if (chkBackground.IsChecked == true)
                rect.Visibility = Visibility.Visible;
            else
                rect.Visibility = Visibility.Hidden;

            if (chkDiffuse.IsChecked == true)
                materialsGroup.Children.Add((Material)FindResource("diffuse"));

            if (chkSpecular.IsChecked == true)
                materialsGroup.Children.Add((Material)FindResource("specular"));

            if (chkEmissive.IsChecked == true)
                materialsGroup.Children.Add((Material)FindResource("emissive"));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
