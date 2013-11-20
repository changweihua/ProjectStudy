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
    /// LoadingSquareUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingSquareUserControl : UserControl
    {

        // Using a DependencyProperty as the backing store for ValueProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("ValueProperty", typeof(double), typeof(LoadingSquareUserControl), new UIPropertyMetadata(.0, (d, p) =>
            {
                LoadingSquareUserControl userControl = d as LoadingSquareUserControl;
                userControl.canvas_Value.Width = 25 * (double)p.NewValue / userControl.MaxValue;
            }));

        public double MaxValue { get; private set; }

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public LoadingSquareUserControl()
        {
            InitializeComponent();
            MaxValue = 10;
        }
    }
}


