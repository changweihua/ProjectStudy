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
using System.Runtime.InteropServices;

namespace FlashGame
{
    /// <summary>
    /// GameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameWindow : Elysium.Controls.Window
    {
        public GameWindow(string path):base()
        {
            InitializeComponent();

            string flashPath = path;
            flashShow.Movie = flashPath;
            flashShow.ScaleMode = 2;
            flashShow.FSCommand += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEventHandler(flashShow_FSCommand);

            this.Wpr = new FlaWndProc(this.FlashWndProc);
            this.OldWndProc = SetWindowLong(flashShow.Handle, GWL_WNDPROC, this.Wpr);
        }

        public GameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //WindowsFormsHost host = new WindowsFormsHost();
            //AxShockwaveFlashObjects.AxShockwaveFlash flash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            //host.Child = flash;

            //myGrid.Children.Add(host);

            //string flashPath = Environment.CurrentDirectory;
            //flashPath += @"\game\car.swf";
            //flash.Movie = flashPath;
            //缩放模式
            //0 ——相当于 Scale 取 "ShowAll" 
            //1 ——相当于 Scale 取 "NoBorder" 
            //2 ——相当于 Scale 取 "ExactFit" 
            //flash.ScaleMode = 2;

            //this.Wpr = new FlaWndProc(this.FlashWndProc);
            //this.OldWndProc = SetWindowLong(flash.Handle, GWL_WNDPROC, this.Wpr);
        }

        //屏蔽FLash菜单
        private const int GWL_WNDPROC = -4;
        public delegate IntPtr FlaWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr iParam);

        private IntPtr OldWndProc = IntPtr.Zero;
        private FlaWndProc Wpr = null;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, FlaWndProc wndProc);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr wmdProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private IntPtr FlashWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == 516)
                return (IntPtr)0;
            return CallWindowProc(OldWndProc, hWnd, msg, wParam, lParam);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow sw = new SettingWindow();
            sw.Owner = this;
            sw.ShowDialog();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show(this, "确定要退出游戏吗", "信息提示", MessageBoxButton.YesNo, MessageBoxImage.Information);

            //if(result == MessageBoxResult.Yes)
            //{
            //    flashShow.Dispose();
            //}
            //else
            //{
            //    e.Cancel = true;
            //}

            ConfirmWindow cw=new ConfirmWindow();
            cw.Owner = this;
            bool? result = cw.ShowDialog();

            if (result == true)
            {
                flashShow.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        void flashShow_FSCommand(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEvent e)
        {
            
        }

    }
}
