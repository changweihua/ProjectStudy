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
    /// LoadingCircleUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingCircleUserControl : UserControl
    {

        public double MaxValue { get; private set; }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("ValueProperty", typeof(double), typeof(LoadingCircleUserControl), new UIPropertyMetadata(.0, (d, p) =>
            {
                LoadingCircleUserControl userControl = d as LoadingCircleUserControl;
                userControl.canvas_Value.Width = 25 * (double)p.NewValue / userControl.MaxValue;
            }));

        public LoadingCircleUserControl()
        {
            InitializeComponent();
            MaxValue = 10;
        }
    }
}
