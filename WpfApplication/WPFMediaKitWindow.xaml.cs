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
using WPFMediaKit.DirectShow.Controls;
using System.IO;

namespace WpfApplication
{
    /// <summary>
    /// WPFMediaKitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WPFMediaKitWindow : Window
    {
        MemoryStream ms = null;
        public byte[] CaptureData { get; set; }

        public WPFMediaKitWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cb.ItemsSource = MultimediaUtil.VideoInputNames;
            if(MultimediaUtil.VideoInputNames.Length>0)
            {
                cb.SelectedIndex = 0;
            }
            else
            {
                
            }
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vce.VideoCaptureSource = (string)cb.SelectedItem;
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            //captureElement. 怎么抓取高清的原始图像
            //能不能抓视频。
            //todo：怎么只抓取一部分
            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)vce.ActualWidth, (int)vce.ActualHeight,
                96, 96, PixelFormats.Default);
            bmp.Render(vce);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                CaptureData = ms.ToArray();
            }
            vce.Pause();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream(@"E:\changweihua\" + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-sss")+".png", FileMode.OpenOrCreate))
            {
                //using (MemoryStream ms = new MemoryStream(CaptureData))
                //{
                //    ms.WriteTo(fs);
                //}
                fs.Write(CaptureData, 0, CaptureData.Length);
            }
        }
    }
}
