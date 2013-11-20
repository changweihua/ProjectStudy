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

namespace FileWall.Controls
{
    /// <summary>
    /// LoadingUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingUserControl : UserControl
    {
        public event EventHandler LoadingCompleted;

        public LoadingUserControl()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(LoadingUserControl_Loaded);
        }

        void LoadingUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(delegate
            {
                int index = 1;
                if (MainWindow.PhotoInfos == null) 
                    return;
                foreach (var photo in MainWindow.PhotoInfos)
                {
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        textBlock_LoadingTips.Text = index.ToString() + @"/" + MainWindow.PhotoInfos.Length.ToString();
                        loadingProgress.Value = (double)index / MainWindow.PhotoInfos.Length * 100;
                    }, null);

                    System.Threading.Thread.Sleep(1000);

                    index++;
                }
                if (LoadingCompleted != null)
                {
                    this.Dispatcher.BeginInvoke((Action)delegate
                    {
                        LoadingCompleted(this, null);
                    }, null);
                }
            })).Start();
        }
    }
}
