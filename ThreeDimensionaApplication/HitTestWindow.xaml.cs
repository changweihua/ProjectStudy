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
    /// HitTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HitTestWindow : Window
    {
        public HitTestWindow()
        {
            InitializeComponent();
        }

        private void Viewport3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Viewport3D viewport = (Viewport3D)sender;

            Point point = e.GetPosition(viewport);

            Console.WriteLine("({0}, {1})", point.X, point.Y);

            HitTestResult result = VisualTreeHelper.HitTest(viewport, point);

            //if (result != null && result.VisualHit == cubeVisual)
            //{
            //    Console.WriteLine("击中立方体");
            //}

            RayMeshGeometry3DHitTestResult meshResult = result as RayMeshGeometry3DHitTestResult;

            if (result != null && meshResult.ModelHit == cubeModel)
            {
                Console.WriteLine("击中立方体1");
            }

            if (result != null && meshResult.MeshHit == cubeGeometry)
            {
                Console.WriteLine("击中立方体2");
            }

        }
    }
}
