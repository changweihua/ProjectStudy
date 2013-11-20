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
using AForge.Video.DirectShow;
using Elysium.Controls;
using AForge.Controls;


namespace FlashGame
{
    /// <summary>
    /// ProtraitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProtraitWindow : Elysium.Controls.Window
    {
        BitmapSource ImagePlay;
        BitmapSource ImageStop;

        bool IsPlaying = false;
        int CurrentIndex = 0;

        public ProtraitWindow()
        {
            InitializeComponent();

            // 设置窗体装载后事件处理器  
            this.Loaded += new RoutedEventHandler(ProtraitWindow_Loaded);
        }

        void ProtraitWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {   // 默认设备  
                sourcePlayer.VideoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            }
            else
            {
                //button_Play.IsEnabled = false;
                //button_Capture.IsEnabled = false;
            }  
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                if (sourcePlayer.IsRunning)
                {   // 停止视频  
                    sourcePlayer.SignalToStop();
                    sourcePlayer.WaitForStop();
                }
                IsPlaying = false;
                ((sender as CommandButton).Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/Icons;Component/wp/dark/appbar.control.play.png"));
            }
            else
            {
                sourcePlayer.Start();
                ((sender as CommandButton).Content as Image).Source = new BitmapImage(new Uri("pack://application:,,,/Icons;Component/wp/dark/appbar.control.stop.png"));
                IsPlaying = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sourcePlayer.IsRunning)
            {   // 停止视频  
                sourcePlayer.SignalToStop();
                sourcePlayer.WaitForStop();
            }  
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            // 判断视频设备是否开启  
            if (sourcePlayer.IsRunning)
            {   // 进行拍照  
                for (Int32 i = 1; i <= 4; i++)
                {
                    object box = this.FindName("pb" + i);
                    if (box is Image )
                    {
                        //为空
                        if ((box as Image).Source == null)
                        {
                            // 更新图像  
                            (box as Image).Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                sourcePlayer.GetCurrentVideoFrame().GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

                            break;
                        }
                       
                    }
                }
            }  
        }
    }
}
