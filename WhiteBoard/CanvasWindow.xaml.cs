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

namespace WhiteBoard
{
    /// <summary>
    /// CanvasWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CanvasWindow : Window
    {
        /// <summary>
        /// 是否正在执行动画
        /// </summary>
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(false));



        public bool IsStopped
        {
            get { return (bool)GetValue(IsStoppedProperty); }
            set { SetValue(IsStoppedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsStopped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsStoppedProperty =
            DependencyProperty.Register("IsStopped", typeof(bool), typeof(MainWindow), new UIPropertyMetadata(true));

        //表示已经展开
        public bool IsToggleButtonExpanded = false;

        //已经展示出菜单
        private int MenuStackPanelShowedIndex = -1;

        public CanvasWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            border.IsEnabled = false;
            //border.BorderBrush = Brushes.Red;
            int currIndex=Convert.ToInt32(border.Tag);

            //显示
            if (MenuStackPanelShowedIndex == -1 || MenuStackPanelShowedIndex != currIndex)
            {
                if (border != null)
                {
                    //恢复所有到默认
                    CollapseALl();

                    TransformGroup group = border.RenderTransform as TransformGroup;
                    var translateTransform = group.Children[0] as TranslateTransform;

                    if (translateTransform != null)
                    {
                        DoubleAnimation doubleAnimation = new DoubleAnimation(0, -45, new Duration(TimeSpan.FromMilliseconds(1000)));
                        doubleAnimation.EasingFunction = new BounceEase() { Bounces = 1, EasingMode = EasingMode.EaseOut, Bounciness = 3 };
                        Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Y"));
                        Storyboard translationStoryboard = new Storyboard();
                        translationStoryboard.Children.Add(doubleAnimation);
                        translationStoryboard.Begin(border);
                    }
                }

                //Point offset = 【Visual】.TransformToAncestor(window).Transform(new Point(0, 0));
                //Visual是你需要获得坐标的对象。

                Point offset = border.TransformToAncestor(this).Transform(new Point(0, 0));//获取元素相对窗体的坐标

                //展开菜单
                var borderPanel = MenuCanvas.Children[Convert.ToInt32(border.Tag)] as StackPanel;
                TransformGroup group2 = borderPanel.RenderTransform as TransformGroup;
                var translate = group2.Children[0] as TranslateTransform;

                if (translate != null)
                {

                    //Canvas.Left 动画
                    DoubleAnimation animation1 = new DoubleAnimation();
                    animation1.From = bottomCanvas.ActualWidth;
                    animation1.To = offset.X + border.ActualWidth + 20;
                    animation1.EasingFunction = new BounceEase() { Bounces = 1, EasingMode = EasingMode.EaseOut, Bounciness = 2 };

                    //borderPanel.BeginAnimation(Canvas.LeftProperty, animation1);

                    //透明度动画
                    DoubleAnimation animation3 = new DoubleAnimation();
                    animation3.To = 1;
                    //borderPanel.BeginAnimation(Border.OpacityProperty, animation3);

                    Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.LeftProperty));
                    Storyboard.SetTargetProperty(animation3, new PropertyPath(Border.OpacityProperty));

                    Storyboard s = new Storyboard();
                    s.Completed += (x, y) =>
                    {
                        border.IsEnabled = true;
                        //border.BorderBrush = Brushes.Navy;
                    };

                    s.Children.Add(animation1);
                    s.Children.Add(animation3);
                    s.Begin(borderPanel);

                }
                MenuStackPanelShowedIndex = currIndex;
            }
            else//隐藏 
            {
                if (border != null)
                {
                    //恢复所有到默认
                    Collapse(border);

                    //菜单
                    var borderPanel = MenuCanvas.Children[Convert.ToInt32(border.Tag)] as StackPanel;
                    TransformGroup group2 = borderPanel.RenderTransform as TransformGroup;
                    var translate = group2.Children[0] as TranslateTransform;

                    if (translate != null)
                    {
                        //Canvas.Left 动画
                        DoubleAnimation animation1 = new DoubleAnimation();
                        animation1.To = 0;

                        //透明度动画
                        DoubleAnimation animation3 = new DoubleAnimation();
                        animation3.To = 0;

                        Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.LeftProperty));
                        Storyboard.SetTargetProperty(animation3, new PropertyPath(Border.OpacityProperty));

                        Storyboard s = new Storyboard();
                        s.Completed += (x, y) =>
                        {
                            border.IsEnabled = true;
                        };

                        s.Children.Add(animation1);
                        s.Children.Add(animation3);
                        s.Begin(borderPanel);

                    }
                }

                
                MenuStackPanelShowedIndex = -1;
            }

            

        }

        void s_Completed(object sender, EventArgs e)
        {
            IsStopped = true;
        }

        void CollapseALl()
        {
            foreach (var item in bottomCanvas.Children)
            {
                if (item.GetType() == typeof(Border))
                {

                    var b = item as Border;

                    if (b == ToggleButtons)
                    {
                        continue;
                    }

                    if (b != null)
                    {
                        TransformGroup group = b.RenderTransform as TransformGroup;
                        var translateTransform = group.Children[0] as TranslateTransform;

                        if (translateTransform != null)
                        {
                            //DoubleAnimation doubleAnimation = new DoubleAnimation();
                            //doubleAnimation.To = 0;
                            //doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                            //Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Y"));
                            //Storyboard translationStoryboard = new Storyboard();
                            //translationStoryboard.Children.Add(doubleAnimation);
                            //translationStoryboard.Begin(b);

                            //DoubleAnimation opacityAnimation = new DoubleAnimation();
                            //opacityAnimation.To = 0.4;
                            //opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                            //Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
                            //Storyboard opacityStoryboard = new Storyboard();
                            //opacityStoryboard.Children.Add(opacityAnimation);
                            //opacityStoryboard.Begin(b);

                            Collapse(b);

                            var menuBorder = MenuCanvas.Children[Convert.ToInt32(b.Tag)] as StackPanel;

                            //Canvas.Left 动画
                            DoubleAnimation animation1 = new DoubleAnimation();
                            animation1.Duration = new Duration(TimeSpan.FromSeconds(0.8));
                            animation1.To = 0;
                            menuBorder.BeginAnimation(Canvas.LeftProperty, animation1);

                            //透明度动画
                            DoubleAnimation animation3 = new DoubleAnimation();
                            animation3.To = 0;
                            animation3.Duration = new Duration(TimeSpan.FromSeconds(0.8));
                            menuBorder.BeginAnimation(Border.OpacityProperty, animation3);
                        }
                    }
                }


            }
        }

        void Collapse(Border border)
        {
            if (border != null)
            {

                TransformGroup group = border.RenderTransform as TransformGroup;
                var translateTransform = group.Children[0] as TranslateTransform;

                if (translateTransform != null)
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.To = 0;
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Y"));

                    DoubleAnimation opacityAnimation = new DoubleAnimation();
                    opacityAnimation.To = 0.4;
                    opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

                    Storyboard storyboard = new Storyboard();
                    storyboard.Children.Add(doubleAnimation);
                    storyboard.Children.Add(opacityAnimation);
                    storyboard.Completed += (a, b) =>
                    {
                        Console.WriteLine(border.Opacity);
                    };
                    storyboard.Begin(border);
                }
            }
        }


        private void ToggleButtons_MouseDown(object sender, MouseButtonEventArgs e)
        {

            int index = 4;
            double beginTime = 0.5;

            if(!IsToggleButtonExpanded)
            {
                ToggleButtonExpand();
            }
            else
            {
                ToggleButtonCollapse();
            }

            return;
           
            foreach (var item in bottomCanvas.Children)
            {
                if (item.GetType() == typeof(Border))
                {
                    var b = item as Border;

                    if (b == ToggleButtons)
                    {
                        continue;
                    }

                    if (index < 0)
                    {
                        break;
                    }

                   this.Dispatcher.BeginInvoke((Action)delegate
                   {

                       var border = bottomCanvas.Children[index] as Border;

                       //Canvas.Left 动画
                       DoubleAnimation animation1 = new DoubleAnimation();
                       animation1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                       animation1.To = 100 * (index + 1);
                       border.BeginAnimation(Canvas.LeftProperty, animation1);

                       //透明度动画
                       DoubleAnimation animation3 = new DoubleAnimation();
                       animation3.To = 1;
                       animation3.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                       border.BeginAnimation(Border.OpacityProperty, animation3);

                       System.Threading.Thread.Sleep(500);

                       index--;
                   });

                   
                }
            }
        }

        private void ToggleButtonExpand()
        {

            int index = 1;
            double beginTime = 1.2;

            //Canvas.Left 动画
            DoubleAnimation leftAnimation = new DoubleAnimation();
            //透明度动画
            DoubleAnimation opacityAnimation = new DoubleAnimation();

            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("Canvas.Left"));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            Storyboard storyBoard = new Storyboard();
            foreach (var item in bottomCanvas.Children)
            {
                if (item.GetType() == typeof(Border))
                {
                    var border = item as Border;

                    if (border == ToggleButtons)
                    {
                        continue;
                    }

                    leftAnimation.Duration = TimeSpan.FromSeconds(0.5);
                    leftAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                    leftAnimation.To = 100 * index++;
                    border.BeginAnimation(Canvas.LeftProperty, leftAnimation);

                    opacityAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                    opacityAnimation.To = 0.4;
                    opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    border.BeginAnimation(Border.OpacityProperty, opacityAnimation);

                    //storyBoard.Children.Add(leftAnimation);
                    //storyBoard.Children.Add(opacityAnimation);

                    beginTime -= 0.3;
                }
            }

            //storyBoard.Completed += storyBoard_Completed;
            //storyBoard.Begin(ToggleButtons);

            IsToggleButtonExpanded = !IsToggleButtonExpanded;

        }

        private void ToggleButtonCollapse()
        {
            //HideMeunStackPanel();
            CollapseALl();

            double beginTime = 0.5;

            //Canvas.Left 动画
            DoubleAnimation leftAnimation = new DoubleAnimation();
            //透明度动画
            DoubleAnimation opacityAnimation = new DoubleAnimation();

            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("Canvas.Left"));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            Storyboard storyBoard = new Storyboard();
            foreach (var item in bottomCanvas.Children)
            {
                if (item.GetType() == typeof(Border))
                {
                    var border = item as Border;

                    if (border == ToggleButtons)
                    {
                        continue;
                    }

                    leftAnimation.Duration = TimeSpan.FromSeconds(0.5);
                    leftAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                    leftAnimation.To = 0;
                    border.BeginAnimation(Canvas.LeftProperty, leftAnimation);

                    opacityAnimation.BeginTime = TimeSpan.FromSeconds(beginTime);
                    opacityAnimation.To = 0;
                    opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    border.BeginAnimation(Border.OpacityProperty, opacityAnimation);

                    beginTime += 0.2;
                }
            }

            IsToggleButtonExpanded = !IsToggleButtonExpanded;
            

        }

        /// <summary>
        /// 隐藏弹出的 MeunStackPanel
        /// </summary>
        void HideMeunStackPanel()
        {
            foreach (var item in MenuCanvas.Children)
            {
                if(item.GetType()==typeof(StackPanel))
                {
                    var sp = item as StackPanel;

                    if(sp.Opacity!=1)
                    {
                        continue;
                    }

                    TransformGroup group2 = sp.RenderTransform as TransformGroup;
                    var translate = group2.Children[0] as TranslateTransform;

                    if (translate != null)
                    {

                        //Canvas.Left 动画
                        DoubleAnimation animation1 = new DoubleAnimation();
                        animation1.To = 0;
                        animation1.EasingFunction = new BounceEase() { Bounces = 1, EasingMode = EasingMode.EaseOut, Bounciness = 2 };
                        sp.BeginAnimation(Canvas.LeftProperty, animation1);

                        //透明度动画
                        DoubleAnimation animation3 = new DoubleAnimation();
                        animation3.To = 0;
                        sp.BeginAnimation(Border.OpacityProperty, animation3);

                    }
                }
            }

        }


        void storyBoard_Completed(object sender, EventArgs e)
        {
            IsToggleButtonExpanded = !IsToggleButtonExpanded;
            ToggleButtons.Opacity = 0.3;
            MessageBox.Show("动画结束");
        }
    }
}
