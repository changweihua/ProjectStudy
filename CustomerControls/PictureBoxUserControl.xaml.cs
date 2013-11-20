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

namespace CustomerControls
{
    /// <summary>
    /// PictureBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PictureBoxUserControl : UserControl
    {

        /// <summary>  
        /// 初始图像  
        /// </summary>  
        private ImageSource _InitialImage = null;  

        public PictureBoxUserControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PictureBoxUserControl_Loaded);
        }

        void PictureBoxUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 设置各元素大小和位置  
            this.image1.Width = this.canvas.Width = this.Width - this.BorderThickness.Left - this.BorderThickness.Right;
            this.image1.Height = this.canvas.Height = this.Height - this.BorderThickness.Top - this.BorderThickness.Bottom;
            Canvas.SetLeft(this.image1, 0);
            Canvas.SetTop(this.image1, 0);

            // 设置图像框伸展属性  
            this.image1.Stretch = System.Windows.Media.Stretch.Fill;
            this.image1.StretchDirection = StretchDirection.Both; 
        }
    }
}
