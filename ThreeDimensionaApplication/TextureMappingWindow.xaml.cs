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
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace ThreeDimensionaApplication
{
    /// <summary>
    /// TextureMappingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TextureMappingWindow : Window
    {
        public TextureMappingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard story = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 360;
            animation.RepeatBehavior = RepeatBehavior.Forever;
            animation.Duration = TimeSpan.FromSeconds(2.5);
            geometry.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        }
    }
}
