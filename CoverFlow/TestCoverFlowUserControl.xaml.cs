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

namespace CoverFlow
{
    /// <summary>
    /// TestCoverFlowUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TestCoverFlowUserControl : Window
    {
        public TestCoverFlowUserControl()
        {
            InitializeComponent();

            this.KeyUp += new KeyEventHandler(TestCoverFlowUserControl_KeyUp);

        }

        /// <summary>
        /// 监听键盘事件，方向键左，元素向左移动，方向键右，元素向右移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TestCoverFlowUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                if (coverflow.IntermediateIndex < coverflow.Count - 4)
                {
                    coverflow.IntermediateIndex++;
                }
            }
            else if (e.Key == Key.Left)
            {
                if (coverflow.IntermediateIndex - 3 > 0)
                {
                    coverflow.IntermediateIndex--;
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //输出更改之后的值
            Debug.WriteLine(e.NewValue);
        }
    }
}
