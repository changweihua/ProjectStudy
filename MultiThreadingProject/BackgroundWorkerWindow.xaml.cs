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
using System.ComponentModel;
using System.Diagnostics;

namespace MultiThreadingProject
{
    /// <summary>
    /// BackgroundWorkerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BackgroundWorkerWindow : Window
    {
        BackgroundWorker worker;

        public BackgroundWorkerWindow()
        {
            InitializeComponent();


            this.Loaded += new RoutedEventHandler(BackgroundWorkerWindow_Loaded);
        }

        void BackgroundWorkerWindow_Loaded(object sender, RoutedEventArgs e)
        {
             worker = this.FindResource("backgroundWorker") as BackgroundWorker;

           

        }

        private void backgroundWorkerControlPanel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;

            if(btn!=null)
            {
                switch (btn.Tag.ToString())
                {
                    case "Start":
                        if (worker != null)
                        {
                            var list = Enumerable.Range(1, 10);
                            worker.RunWorkerAsync(list);
                        }
                        break;
                    case "Cancel":
                        if (worker != null)
                        {
                            worker.CancelAsync();
                        }
                        break;
                    case "Pause":
                        if (worker != null)
                        {
                           
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IEnumerable<int> list = e.Argument as IEnumerable<int>;

            Print(list);

            if(worker.CancellationPending)
            {
                e.Result = "用户取消操作";
                return;
            }

            e.Result = "输出完毕，总计 " + list.Count() + " 个元素";
        }

        void Print(IEnumerable<int> list)
        {
            int i = 10;
            foreach (var item in list)
            {
                if (worker.CancellationPending)
                {
                    return;
                }
                Console.WriteLine(item);
                worker.ReportProgress(i, null);
                i += 10;
                System.Threading.Thread.Sleep(2000);//如果不加这句，会导致输出结果，然后才输出进度
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(e.Result.ToString());
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("当前进度 " + e.ProgressPercentage + "%");
        }
    }
}
