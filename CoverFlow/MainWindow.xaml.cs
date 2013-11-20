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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using _3DTools;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace CoverFlow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 当前中间索引
        /// </summary>
        public int CurrentMiddleIndex
        {
            get { return (int)GetValue(CurrentMiddleIndexProperty); }
            set { SetValue(CurrentMiddleIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentMiddleIndexProperty =
            DependencyProperty.Register("CurrentMiddleIndex", typeof(int), typeof(MainWindow), new FrameworkPropertyMetadata(new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));

        private static void CurrentMidIndexPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            MainWindow win = sender as MainWindow;
            if (win != null)
            {
                win.ReLayoutInteractiveVisual3D();
            }
        }

        /// <summary>
        /// 选择角度
        /// </summary>
        public double ModelAngle
        {
            get { return (double)GetValue(ModelAngleProperty); }
            set { SetValue(ModelAngleProperty, value); }
        }

        public static readonly DependencyProperty ModelAngleProperty =
            DependencyProperty.Register("ModelAngle", typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(70.0,new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));


        /// <summary>
        /// 获取或设置Z方向上两个模型间的距离
        /// </summary>
        public double ZDistanceOfModel
        {
            get { return (double)GetValue(ZDistanceOfModelProperty); }
            set { SetValue(ZDistanceOfModelProperty, value); }
        }

        public static readonly DependencyProperty ZDistanceOfModelProperty =
            DependencyProperty.Register("ZDistanceOfModel", typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0.5, new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));


        /// <summary>
        /// 获取或设置X方向上两个模型间的距离
        /// </summary>
        public double XDistanceOfModel
        {
            get { return (double)GetValue(XDistanceOfModelProperty); }
            set { SetValue(XDistanceOfModelProperty, value); }
        }

        public static readonly DependencyProperty XDistanceOfModelProperty =
            DependencyProperty.Register("XDistanceOfModel", typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(0.5,new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));


        /// <summary>
        /// 获取或设置中间的模型距离两边模型的距离
        /// </summary>
        public double ZDistanceFromMiddle
        {
            get { return (double)GetValue(ZDistanceFromMiddleProperty); }
            set { SetValue(ZDistanceFromMiddleProperty, value); }
        }

        public static readonly DependencyProperty ZDistanceFromMiddleProperty =
            DependencyProperty.Register("ZDistanceFromMiddle", typeof(double), typeof(MainWindow), new FrameworkPropertyMetadata(1.5, new PropertyChangedCallback(CurrentMidIndexPropertyChangedCallback)));


        

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (sender, e) => {
                Debug.WriteLine("进入窗体加载事件");
                this.LoadImageToViewport3D(LoadImage(@"C:\Users\Public\Pictures\Sample Pictures\"));
            };

            this.MouseDown += (sender, e) => {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    CurrentMiddleIndex++;
                }
                else
                {
                    CurrentMiddleIndex--;
                }
            };
        }

        /// <summary>
        /// 重新布局3D内容
        /// </summary>
        void ReLayoutInteractiveVisual3D()
        {
            Debug.WriteLine("重新布局3D内容");

            int j = 0;
            for (int i = 0; i < this.viewport3D.Children.Count; i++)
            {
                InteractiveVisual3D iv3d = this.viewport3D.Children[i] as InteractiveVisual3D;

                if (iv3d != null)
                {
                    double angle = 0;
                    double offsetX = 0;
                    double offsetZ = 0;

                    GetTransformOfInteractiveVisual3D(j++, CurrentMiddleIndex, out angle, out offsetX, out offsetZ);

                    NameScope.SetNameScope(this, new NameScope());
                    this.RegisterName("iv3d", iv3d);
                    Duration time = new Duration(TimeSpan.FromSeconds(0.3));

                    DoubleAnimation angleAnimation = new DoubleAnimation(angle, time);
                    DoubleAnimation xAnimation = new DoubleAnimation(offsetX, time);
                    DoubleAnimation zAnimation = new DoubleAnimation(offsetZ, time);

                    Storyboard story = new Storyboard();
                    story.Children.Add(angleAnimation);
                    story.Children.Add(xAnimation);
                    story.Children.Add(zAnimation);

                    Storyboard.SetTargetName(angleAnimation, "iv3d");
                    Storyboard.SetTargetName(xAnimation, "iv3d");
                    Storyboard.SetTargetName(zAnimation, "iv3d");

                    Storyboard.SetTargetProperty(
                        angleAnimation,
                        new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[0].(RotateTransform3D.Rotation).(AxisAngleRotation3D.Angle)"));

                    Storyboard.SetTargetProperty(
                        xAnimation,
                        new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[1].(TranslateTransform3D.OffsetX)"));
                    Storyboard.SetTargetProperty(
                        zAnimation,
                        new PropertyPath("(ModelVisual3D.Transform).(Transform3DGroup.Children)[1].(TranslateTransform3D.OffsetZ)"));

                    story.Begin(this);


                }

            }

        }

        /// <summary>
        /// 依照InteractiveVisual3D在列表中的序号来变换其位置等
        /// </summary>
        /// <param name="index">在列表中的序号</param>
        /// <param name="midIndex">列表中被作为中间项的序号</param>
        /// <param name="angle"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetZ"></param>
        void GetTransformOfInteractiveVisual3D(int index,double midIndex,out double angle, out double offsetX, out double offsetZ)
        {
            Debug.WriteLine("依照InteractiveVisual3D在列表中的序号来变换其位置等");

            double offsetIndex = index - midIndex;
            //旋转，两翼的图片各旋转一定的度数
            angle = 0;
            if (offsetIndex < 0)
            {
                angle = ModelAngle;
            }
            else if (offsetIndex > 0)
            {
                angle = -ModelAngle;
            }

            offsetX = 0;
            if (Math.Abs(offsetIndex) <= 1)
            {
                offsetX = offsetIndex * ZDistanceFromMiddle;
            }
            else if (Math.Abs(offsetIndex) != 0)
            {
                offsetX = offsetIndex * XDistanceOfModel + (offsetIndex > 0 ? ZDistanceFromMiddle : -ZDistanceFromMiddle);
            }

            //两翼的图片逐渐向Z轴负方向移动一点,造成中间突出(离观众较近的效果)
            offsetZ = Math.Abs(offsetIndex) * -ZDistanceOfModel;

        }

        /// <summary>
        /// 添加图片到视口
        /// </summary>
        /// <param name="images"></param>
        void LoadImageToViewport3D(List<string> images)
        {
            Debug.WriteLine("添加图片到视口");
            if (images == null)
            {
                return;
            }

            for (int i = 0; i < images.Count; i++)
            {
                string imageFile = images[i];

                InteractiveVisual3D iv3d = this.CreateInteractiveVisual3D(imageFile, i);

                this.viewport3D.Children.Add(iv3d);
            }

            ReLayoutInteractiveVisual3D();
        }

        /// <summary>
        /// 创建 3D 图形
        /// </summary>
        /// <returns></returns>
        Geometry3D CreateGeometry3D()
        {
            Debug.WriteLine("创建 3D 图形");
            MeshGeometry3D geometry = new MeshGeometry3D();

            geometry.Positions = new Point3DCollection();
            geometry.Positions.Add(new Point3D(-1, 1, 0));
            geometry.Positions.Add(new Point3D(-1, -1, 0));
            geometry.Positions.Add(new Point3D(1, -1, 0));
            geometry.Positions.Add(new Point3D(1, 1, 0));

            geometry.TriangleIndices = new Int32Collection();
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(1);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(3);

            geometry.TextureCoordinates = new PointCollection();
            geometry.TextureCoordinates.Add(new Point(0, 0));
            geometry.TextureCoordinates.Add(new Point(0, 1));
            geometry.TextureCoordinates.Add(new Point(1, 1));
            geometry.TextureCoordinates.Add(new Point(1, 0));

            return geometry;
        }
        
        /// <summary>
        /// 创建一个空的Transform3DGroup
        /// </summary>
        /// <returns></returns>
        Transform3DGroup CreateEmptyTransform3DGroup()
        {
            Debug.WriteLine("创建一个空的Transform3DGroup");

            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0)));
            group.Children.Add(new TranslateTransform3D(new Vector3D()));
            group.Children.Add(new ScaleTransform3D());

            return group;
        }
        
        /// <summary>
        /// 为指定图片路径创建一个3D视觉对象
        /// </summary>
        /// <returns></returns>
        InteractiveVisual3D CreateInteractiveVisual3D(string path, int index)
        {
            Debug.WriteLine("为指定图片路径创建一个3D视觉对象");

            InteractiveVisual3D visual3D = new InteractiveVisual3D();

            visual3D.Visual = CreateVisual(path, index);
            visual3D.Geometry = CreateGeometry3D();
            visual3D.Transform = CreateEmptyTransform3DGroup();

            return visual3D;
        }

        /// <summary>
        /// 由指定的图片路径创建一个可视对象
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="index">索引</param>
        /// <returns>可视化对象</returns>
        Visual CreateVisual(string path, int index)
        {
            Debug.WriteLine("由指定的图片路径创建一个可视对象");

            BitmapImage bitmapImage = null;

            if (!File.Exists(path))
            {
                return null;
            }

            bitmapImage = new BitmapImage(new Uri(path));

            Image image = new Image();
            image.Width = 50;
            image.Source = bitmapImage;

            Border border = new Border();
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(3);
            border.Child = image;

            border.MouseDown += (sender, e) => {
                this.CurrentMiddleIndex = index;
                e.Handled = true;
            };

            return border;
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="path">图片路径，即文件夹</param>
        /// <returns>图片路径集合</returns>
        List<string> LoadImage(string path)
        {
            Debug.WriteLine("加载图片");

            List<string> list = null;

            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                var files = dirInfo.GetFiles("*.jpg", SearchOption.AllDirectories);

                if (files != null)
                {
                    list = new List<string>();
                    list = (from file in files select file.FullName).ToList();
                }

            }

            return list;
        }
    }
}
