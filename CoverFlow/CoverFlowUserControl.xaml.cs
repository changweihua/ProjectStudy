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
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Media3D;
using _3DTools;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace CoverFlow
{
    /// <summary>
    /// CoverFlowUserControl.xaml 的交互逻辑
    /// 
    /// Todo: 将同步改为异步加载资源
    /// 
    /// </summary>
    public partial class CoverFlowUserControl : UserControl
    {
        /// <summary>
        /// 中间索引两侧展示数量
        /// </summary>
        int SideCount = 3;
        /// <summary>
        /// 资源索引集合
        /// </summary>
        IEnumerable<int> Values = null;

        #region 构造函数

        public CoverFlowUserControl()
        {
            InitializeComponent();

            this.Loaded += (sender, e) =>
            {
                DebugMessages = new ObservableCollection<DebugMessage>();
                DebugMessages.Add(new DebugMessage { ActionName = "CoverFlowUserControl", ActionTime = DateTime.Now.ToString(), ActionMessage = "开始努力的加载资源" }); 
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.LoadModelToViewport3D(LoadImage(SourcePath).ToList());
                    Message = "资源加载完毕，可以开始进行捕捉";
                }), System.Windows.Threading.DispatcherPriority.Loaded);
            };

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 得到索引集合
        /// </summary>
        /// <param name="index">中间索引值</param>
        /// <param name="result">需要展示的索引集合</param>
        /// <param name="others">不需要展示的索引集合</param>
        void GetList(int index, out IEnumerable<int> result, out IEnumerable<int> others)
        {
            result = GetList(index);
            others = Values.Except(result);
        }

        /// <summary>
        /// 得到需要展示的索引集合
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IEnumerable<int> GetList(int index)
        {
            IEnumerable<int> result = null;

            if (index < SideCount)
            {
                result = Values.Reverse().Take(SideCount - index).Reverse().Concat(Values.Take(index + SideCount + 1));
            }
            else if (index > (Count - 1 - SideCount))
            {
                result = Values.Skip(index - SideCount).Concat(Values.Take(index - (Count - 1 - SideCount)));
            }
            else
            {
                result = Values.Skip(index - SideCount).Take(SideCount * 2 + 1);
            }

            return result;
        }
        

        /// <summary>
        /// 重新布局3D内容，实现循环展示
        /// </summary>
        void ReLayoutInteractiveVisual3D(List<int> list)
        {
            var ivs = this.viewport3D.Children.OfType<InteractiveVisual3D>().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                InteractiveVisual3D iv3d = ivs[list[i]] as InteractiveVisual3D;

                if (iv3d != null)
                {
                    double angle = 0;
                    double offsetX = 0;
                    double offsetZ = 0;

                    GetTransformOfInteractiveVisual3D(i, out angle, out offsetX, out offsetZ);

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
        /// 给不需要显示的元素的OffsetX设置足够大的值
        /// </summary>
        /// <param name="list"></param>
        void ReSetInteractiveVisual3D(List<int> list)
        {
            var ivs = this.viewport3D.Children.OfType<InteractiveVisual3D>().ToList();
            
            for (int i = 0; i < list.Count; i++)
            {

                InteractiveVisual3D iv3d = ivs[list[i]] as InteractiveVisual3D;

                if (iv3d != null)
                {
                    double angle = 0;
                    double offsetX = 500;
                    double offsetZ = 0;

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
        /// 得到变换参数值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="angle"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetZ"></param>
        void GetTransformOfInteractiveVisual3D(int index, out double angle, out double offsetX, out double offsetZ)
        {
            int midIndex = 3;
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
                offsetX = offsetIndex * ModelFromCenter;
            }
            else if (Math.Abs(offsetIndex) != 0)
            {
                //修正索引偏移
                offsetIndex = offsetIndex < 0 ? offsetIndex + 1 : offsetIndex - 1;
                //计算 OffsetX 值
                offsetX = offsetIndex * DistanceBetweenModel + (offsetIndex > 0 ? ModelFromCenter : -ModelFromCenter);
            }

            offsetZ = -ZDistance;

        }

        /// <summary>
        /// 重新布局3D内容
        /// </summary>
        void ReLayoutInteractiveVisual3D()
        {
            Debug.WriteLine("重新布局3D内容");
            Debug.WriteLine("{0} ",  this.viewport3D.Children.Count);

            int j = 0;
            for (int i = 0; i < this.viewport3D.Children.Count; i++)
            {
                InteractiveVisual3D iv3d = this.viewport3D.Children[i] as InteractiveVisual3D;

                if (iv3d != null)
                {
                    double angle = 0;
                    double offsetX = 0;
                    double offsetZ = 0;

                    GetTransformOfInteractiveVisual3D(j++, IntermediateIndex, out angle, out offsetX, out offsetZ);

                    //保证中间图片两侧最多只有3张图片
                    if (i < IntermediateIndex - 2 || i > IntermediateIndex + 4)
                    {
                        Debug.WriteLine("隐藏不需要展示的资源");
                        angle = 0;
                        offsetX = 500;
                        offsetZ = -1;
                    }

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
        void GetTransformOfInteractiveVisual3D(int index, double midIndex, out double angle, out double offsetX, out double offsetZ)
        {
            
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
                offsetX = offsetIndex * ModelFromCenter;
            }
            else if (Math.Abs(offsetIndex) != 0)
            {
                //修正索引偏移
                offsetIndex = offsetIndex < 0 ? offsetIndex + 1 : offsetIndex - 1;
                //计算 OffsetX 值
                offsetX = offsetIndex * DistanceBetweenModel + (offsetIndex > 0 ? ModelFromCenter : -ModelFromCenter);
            }

            offsetZ =  -ZDistance;

        }

        /// <summary>
        /// 加载模型到3D视口
        /// </summary>
        /// <param name="source"></param>
        private void LoadModelToViewport3D(IList<string> source)
        {
            if (source == null || source.Count == 0)
            {
                return;
            }

            for (int i = 0; i < source.Count; i++)
            {
                string imageFile = source[i];

                InteractiveVisual3D iv3d = this.CreateInteractiveVisual3D(imageFile, i);

                this.viewport3D.Children.Add(iv3d);
            }
            //确定中间索引
            IntermediateIndex = Count / 2;

        }

        
        #endregion

        #region 为指定图片路径创建一个3D视觉对象

        /// <summary>
        /// 为指定图片路径创建一个3D视觉对象
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        InteractiveVisual3D CreateInteractiveVisual3D(string path, int index)
        {
           
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
            border.BorderBrush = ModelBorderBrush;
            border.BorderThickness = new Thickness(1);
            border.Child = image;
            border.Background = Brushes.Transparent;//防止有时候无法捕捉到触摸事件
           

            border.MouseDown += (sender, e) =>
            {
                Debug.WriteLine("中间值 {0}, 当前索引 {1}", Count / 2, index);
                Debug.WriteLine("距离最小索引值长度 {0}, 距离最大索引值 {1}", index, Count - index - 1);

                this.IntermediateIndex = index;

                e.Handled = true;
            };

            // //启用手势操作
            //border.IsManipulationEnabled = true;

            //border.ManipulationStarting += (object sender, ManipulationStartingEventArgs e) =>
            //{
            //    e.ManipulationContainer = this;
            //    e.Mode = ManipulationModes.All;
            //};

            //border.ManipulationDelta += (sender, e) =>
            //{
            //};

            //border.ManipulationCompleted += (sender, e) =>
            //{
            //    e.Handled = true;
            //};

            image.TouchDown += (sender, e) =>
            {
                startPoint = e.GetTouchPoint(border);
                Message = "开始捕捉";
            };

            image.TouchUp += (sender, e) =>
            {
                var point = e.GetTouchPoint(border);
                Debug.WriteLine("容器实际宽度 {1}, 触摸点水平偏移 {0}", point.Position.X - startPoint.Position.X, border.ActualWidth);
                LogHelper.Log(string.Format("Border 实际宽度 {1}, 触摸点水平偏移 {0}", point.Position.X - startPoint.Position.X, border.ActualWidth));
                Message = string.Format("Border 实际宽度 {1},起始点 = {2}, 结束点 = {3}, 触摸点水平偏移 {0}", point.Position.X - startPoint.Position.X, border.ActualWidth, startPoint.Position.X, point.Position.X);
                //this.Dispatcher.Invoke(new Action(() => { tbMessage.Text = ""; }));
                //Debug.WriteLine("中间值 {0}, 当前索引 {1}", Count / 2, index);
                //Debug.WriteLine("距离最小索引值长度 {0}, 距离最大索引值 {1}", index, Count - index - 1);
                if (point.Position.X - startPoint.Position.X > 0 && Math.Abs(point.Position.X - startPoint.Position.X) > border.ActualWidth / 2)
                {
                    IntermediateIndex = index - 1;
                }
                else if (point.Position.X - startPoint.Position.X < 0 && Math.Abs(point.Position.X - startPoint.Position.X) > border.ActualWidth / 2)
                {
                    IntermediateIndex = index + 1;
                }
                e.Handled = true;
            };

            return border;
        }


        /// <summary>
        /// 创建 3D 图形
        /// </summary>
        /// <returns></returns>
        Geometry3D CreateGeometry3D()
        {
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
            
            Transform3DGroup group = new Transform3DGroup();
            group.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0)));
            group.Children.Add(new TranslateTransform3D(new Vector3D()));
            group.Children.Add(new ScaleTransform3D());

            return group;
        }


        #endregion

        #region 资源加载方法

        /// <summary>
        /// 根据指定的数据源加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> LoadImage(string path)
        {
            DebugMessages.Add(new DebugMessage { ActionName = "LoadImage", ActionTime = DateTime.Now.ToString(), ActionMessage = "加载图片" });
            List<string> list = null;

            if (Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                var files = dirInfo.GetFiles(SourceFilter, SearchOption.AllDirectories);

                if (files != null)
                {
                    list = new List<string>();
                    list = (from file in files select file.FullName).ToList();
                }
                Count = list.Count;
                Values = Enumerable.Range(0, Count);
                Debug.WriteLine("成功加载加载 {0} 图片", Count);
            }
            else
            {
                throw new Exception("无法找到指定路径的资源");
            }

            return list;
        }


        #endregion

        #region 公共事件

        /// <summary>
        /// 自定义依赖属性值修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void CustomPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            CoverFlowUserControl uc = sender as CoverFlowUserControl;
            if (uc != null)
            {
                if (arg.Property.Name == "IntermediateIndex")
                {
                    int index = (int)arg.NewValue;
                    IEnumerable<int> result = null;
                    IEnumerable<int> others = null;
                    uc.GetList(index, out result, out others);
                    uc.ReLayoutInteractiveVisual3D(result.ToList());
                    uc.ReSetInteractiveVisual3D(others.ToList());
                }
            }
        }

        #endregion

        #region 依赖属性


        /// <summary>
        /// 所有操作的调试信息
        /// </summary>
        public ObservableCollection<DebugMessage> DebugMessages
        {
            get { return (ObservableCollection<DebugMessage>)GetValue(DebugMessagesProperty); }
            set { SetValue(DebugMessagesProperty, value); }
        }

        public static readonly DependencyProperty DebugMessagesProperty =
            DependencyProperty.Register("DebugMessages", typeof(ObservableCollection<DebugMessage>), typeof(CoverFlowUserControl), new UIPropertyMetadata(null));


        /// <summary>
        /// 每一个操作的信息，方便调试
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(CoverFlowUserControl), new UIPropertyMetadata("尚未捕捉到"));


        /// <summary>
        /// 当前索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(CustomPropertyChangedCallback)));

        /// <summary>
        /// 中间索引
        /// </summary>
        public int IntermediateIndex
        {
            get { return (int)GetValue(IntermediateIndexProperty); }
            set { SetValue(IntermediateIndexProperty, value); }
        }

        public static readonly DependencyProperty IntermediateIndexProperty =
            DependencyProperty.Register("IntermediateIndex", typeof(int), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(CustomPropertyChangedCallback)));

        /// <summary>
        /// 两个模型之间的距离，即 X 轴
        /// </summary>
        public double DistanceBetweenModel
        {
            get { return (double)GetValue(DistanceBetweenModelProperty); }
            set { SetValue(DistanceBetweenModelProperty, value); }
        }

        public static readonly DependencyProperty DistanceBetweenModelProperty =
            DependencyProperty.Register("DistanceBetweenModel", typeof(double), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(70.0d, new PropertyChangedCallback(CustomPropertyChangedCallback)));


        /// <summary>
        /// 模型偏转角度
        /// </summary>
        public double ModelAngle
        {
            get { return (double)GetValue(ModelAngleProperty); }
            set { SetValue(ModelAngleProperty, value); }
        }

        public static readonly DependencyProperty ModelAngleProperty =
            DependencyProperty.Register("ModelAngle", typeof(double), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(0.5d, new PropertyChangedCallback(CustomPropertyChangedCallback)));


        /// <summary>
        /// 中间点两边模型距离中心点的距离
        /// </summary>
        public double ModelFromCenter
        {
            get { return (double)GetValue(ModelFromCenterProperty); }
            set { SetValue(ModelFromCenterProperty, value); }
        }

        public static readonly DependencyProperty ModelFromCenterProperty =
            DependencyProperty.Register("ModelFromCenter", typeof(double), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(0.5d, new PropertyChangedCallback(CustomPropertyChangedCallback)));


        /// <summary>
        /// 需要展示的资源路径
        /// </summary>
        public string SourcePath
        {
            get { return (string)GetValue(SourcePathProperty); }
            set { SetValue(SourcePathProperty, value); }
        }

        public static readonly DependencyProperty SourcePathProperty =
            DependencyProperty.Register("SourcePath", typeof(string), typeof(CoverFlowUserControl), new UIPropertyMetadata(""));

        /// <summary>
        /// 呈现远近的效果
        /// </summary>
        public double ZDistance
        {
            get { return (double)GetValue(ZDistanceProperty); }
            set { SetValue(ZDistanceProperty, value); }
        }

        public static readonly DependencyProperty ZDistanceProperty =
            DependencyProperty.Register("ZDistance", typeof(double), typeof(CoverFlowUserControl), new FrameworkPropertyMetadata(1.5d, new PropertyChangedCallback(CustomPropertyChangedCallback)));

        /// <summary>
        /// 模型边框颜色
        /// </summary>
        public Brush ModelBorderBrush
        {
            get { return (Brush)GetValue(ModelBorderBrushProperty); }
            set { SetValue(ModelBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty ModelBorderBrushProperty =
            DependencyProperty.Register("ModelBorderBrush", typeof(Brush), typeof(CoverFlowUserControl), new UIPropertyMetadata(Brushes.NavajoWhite));


        /// <summary>
        /// 资源过滤规则
        /// </summary>
        public string SourceFilter
        {
            get { return (string)GetValue(SourceFilterProperty); }
            set { SetValue(SourceFilterProperty, value); }
        }

        public static readonly DependencyProperty SourceFilterProperty =
            DependencyProperty.Register("SourceFilter", typeof(string), typeof(CoverFlowUserControl), new UIPropertyMetadata("*.jpg"));



        /// <summary>
        /// 显示数量，为奇数
        /// </summary>
        public int ShowingCount
        {
            get { return (int)GetValue(ShowingCountProperty); }
            set { SetValue(ShowingCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowingCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowingCountProperty =
            DependencyProperty.Register("ShowingCount", typeof(int), typeof(CoverFlowUserControl), new UIPropertyMetadata(7));


        
      
        #endregion

        #region CLR 属性

        /// <summary>
        /// 资源数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 起始点，用户按下去的初始点位置
        /// </summary>
        private TouchPoint startPoint;

        #endregion

    }
}
