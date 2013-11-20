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
    /// FileElementUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class FileElementUserControl : UserControl
    {
        

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(FileElementUserControl), new UIPropertyMetadata(string.Empty, (d, p) =>
            {
                string fileName = p.NewValue.ToString();
                //if (System.IO.File.Exists(fileName))
                //{
                //    (d as FileElementUserControl).image_Displayer.Source = App.FileModels.Single(_ => _.FullName.Equals(fileName)).Thumbnail;
                //    (d as FileElementUserControl).textBlock_FileName.Text = fileName.FileName();
                //}
            }));

        public FileElementUserControl()
        {
            InitializeComponent();
        }
    }
}
