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
using System.Diagnostics;

namespace WpfApplication
{
    /// <summary>
    /// ClipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClipWindow : Window
    {
        /// <summary>
        /// 移动标识
        /// </summary>
        bool trackingMouseMove = false;
        /// <summary>
        /// 鼠标按下去的位置
        /// </summary>
        Point mousePosition;

        /// <summary>
        /// 调整大图
        /// </summary>
        void AdjustLargeImage()
        {
            ///获取比例
            double n = LargeBox.Width / ClipRect.Width;

            double left = (double)ClipRect.GetValue(Canvas.LeftProperty);
            double top = (double)ClipRect.GetValue(Canvas.TopProperty);
            
            left = -left * n;
            top = -top * n;
            
            LargeImage.SetValue(Canvas.LeftProperty, left);
            LargeImage.SetValue(Canvas.TopProperty, top);

        }

        public ClipWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ClipWindow_Loaded);
        }

        void ClipWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustLargeImage();
        }

        private void ClipRect_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            if (trackingMouseMove)
            {
                double deltaY = e.GetPosition(element).Y - mousePosition.Y;
                double deltaX = e.GetPosition(element).X - mousePosition.X;

                double top = deltaY + (double)element.GetValue(Canvas.TopProperty);
                double left = deltaX + (double)element.GetValue(Canvas.LeftProperty);

                left = left <= 0 ? 0 : left;

                if (left >= (SmallBox.Width - ClipRect.Width))
                {
                    left = SmallBox.Width - ClipRect.Width;
                }

                top = top <= 0 ? 0 : top;

                if (top >= (SmallBox.Height - ClipRect.Height))
                {
                    top = SmallBox.Height - ClipRect.Height;
                }

                #if DEBUG

                System.Diagnostics.Debug.WriteLine("Top = {0}, Left = {1})", top, left);

                #endif


                element.SetValue(Canvas.TopProperty, top);
                element.SetValue(Canvas.LeftProperty, left);
                AdjustLargeImage();
                if (mousePosition.X <= 0 || mousePosition.Y <= 0)
                {
                    return;
                }
            }

        }

        private void ClipRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            mousePosition = e.GetPosition(element);
            trackingMouseMove = true;
            if (element != null)
            {
                //强制获取此元素
                element.CaptureMouse();
                element.Cursor = Cursors.Hand;
            }
        }

        private void ClipRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            trackingMouseMove = false;
            element.ReleaseMouseCapture();
            mousePosition.X = mousePosition.Y = 0;
            element.Cursor = null;
        }
    }
}
