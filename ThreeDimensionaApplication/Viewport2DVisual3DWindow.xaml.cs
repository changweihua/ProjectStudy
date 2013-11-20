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

namespace ThreeDimensionaApplication
{
    /// <summary>
    /// Viewport2DVisual3DWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Viewport2DVisual3DWindow : Window
    {
        public Viewport2DVisual3DWindow()
        {
            InitializeComponent();
        }

        private void cmd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked.");
        }
    }
}
