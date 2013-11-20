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
using System.Windows.Media.Animation;

namespace WhiteBoard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 
    /// Todo    实现判断，点击任一按钮，其余按钮恢复原状并收起菜单
    /// Todo    实现工具栏和按钮的一一对应 
    /// 
    /// </summary>
    public partial class MainWindow : Window
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



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
           

            var border = sender as Border;
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
                //borderPanel.Margin = new Thickness(offset.X, offset.Y, 0, 0);
                //borderPanel.RenderTransformOrigin = offset;
                
                //Canvas.Left 动画
                DoubleAnimation animation1 = new DoubleAnimation();
                animation1.From = bottomStackPanel.ActualWidth;
                animation1.To = offset.X + border.ActualWidth + 20;
                animation1.EasingFunction = new BounceEase() { Bounces = 1, EasingMode = EasingMode.EaseOut, Bounciness = 2 };
                borderPanel.BeginAnimation(Canvas.LeftProperty, animation1);

                //Canvas.Top 动画
                //DoubleAnimation animation2 = new DoubleAnimation();
                //animation2.To = offset.Y - border.ActualHeight / 2 - 30;
                //animation2.EasingFunction = new BounceEase() { Bounces = 1, EasingMode = EasingMode.EaseOut, Bounciness = 3 };
                //borderPanel.BeginAnimation(Canvas.TopProperty, animation2);

                //透明度动画
                DoubleAnimation animation3 = new DoubleAnimation();
                animation3.To = 1;
                borderPanel.BeginAnimation(Border.OpacityProperty, animation3);

                //storyboard3.Begin(borderPanel);
                
            }

        }


        void CollapseALl()
        {
            foreach (var item in bottomStackPanel.Children)
            {
                if (item.GetType() == typeof(Border))
                {
                   
                    var b = item as Border;

                    if(b == ToggleButtons)
                    {
                        continue;
                    }

                    if (b != null)
                    {
                        TransformGroup group = b.RenderTransform as TransformGroup;
                        var translateTransform = group.Children[0] as TranslateTransform;

                        if (translateTransform != null)
                        {
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.To = 0;
                            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Y"));
                            Storyboard translationStoryboard = new Storyboard();
                            translationStoryboard.Children.Add(doubleAnimation);
                            translationStoryboard.Begin(b);

                            DoubleAnimation opacityAnimation = new DoubleAnimation();
                            opacityAnimation.To = 0.4;
                            opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
                            Storyboard opacityStoryboard = new Storyboard();
                            opacityStoryboard.Children.Add(opacityAnimation);
                            opacityStoryboard.Begin(b);

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
            foreach (var item in bottomStackPanel.Children)
            {

                if (item.GetType() == typeof(Border) && border.Tag.ToString() != (item as Border).Tag.ToString())
                {
                    Console.WriteLine((item as Border).Tag + "," + border.Tag);

                    var b = item as Border;
                    if (b != null)
                    {

                        TransformGroup group = b.RenderTransform as TransformGroup;
                        var translateTransform = group.Children[0] as TranslateTransform;

                        if (translateTransform != null)
                        {
                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.To = 45;
                            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Y"));
                            Storyboard translationStoryboard = new Storyboard();
                            translationStoryboard.Children.Add(doubleAnimation);
                            translationStoryboard.Begin(b);
                        }
                    }
                }

               
            }
        }

        private void ToggleButtons_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
