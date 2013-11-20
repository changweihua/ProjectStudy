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
using System.Windows.Media.Animation;

namespace FileWall.Controls
{
    /// <summary>
    /// LoadingProgressUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingProgressUserControl : UserControl
    {
        private double m_Value = 0;
        private LoadingSquareUserControl[] m_LoadingSquareCollection = null;
        private Storyboard m_ProgressAnimation = null;

        public double MaxValue { get; private set; }

        public LoadingSquareUserControl[] LoadingSquareCollection
        {
            get
            {
                if (m_LoadingSquareCollection == null)
                {
                    m_LoadingSquareCollection = new LoadingSquareUserControl[10];
                    for (int i = 0; i < 10; i++)
                    {
                        m_LoadingSquareCollection[i] = uniformGrid_UserControlCollection.Children[i] as LoadingSquareUserControl;//将动画应用于此元素的指定依赖项属性 。 所有现有动画使用新动画停止并替换。
                    }
                }
                return m_LoadingSquareCollection;
            }
        }

        public double Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                int leftSquare = (int)value / 10;
                double currentValue = value % 10;
                for (int i = 0; i < leftSquare; i++)
                {
                    LoadingSquareCollection[i].ApplyAnimationClock(LoadingSquareUserControl.ValueProperty, null);
                    LoadingSquareCollection[i].Value = 10;
                }
                if (leftSquare < 10)
                {
                    if (m_ProgressAnimation != null)
                        m_ProgressAnimation.Stop();

                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = LoadingSquareCollection[leftSquare].Value,
                        To = currentValue,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };

                    Storyboard.SetTarget(animation, LoadingSquareCollection[leftSquare]);
                    Storyboard.SetTargetProperty(animation, new PropertyPath(LoadingSquareUserControl.ValueProperty));
                    m_ProgressAnimation = new Storyboard();
                    m_ProgressAnimation.Children.Add(animation);
                    m_ProgressAnimation.Completed += delegate
                    {
                        if (LoadingSquareCollection[leftSquare].HasAnimatedProperties)
                        {
                            LoadingSquareCollection[leftSquare].ApplyAnimationClock(LoadingSquareUserControl.ValueProperty, null);//将动画应用于此元素的指定依赖项属性 。 所有现有动画使用新动画停止并替换。
                            LoadingSquareCollection[leftSquare].Value = animation.To.Value;
                        }
                        m_ProgressAnimation = null;
                    };
                    m_ProgressAnimation.Begin();

                }
            }
        }

        public LoadingProgressUserControl()
        {
            InitializeComponent();
            MaxValue = 10;
        }
    }
}
