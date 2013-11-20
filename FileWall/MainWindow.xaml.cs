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
using FileWall.Controls;

namespace FileWall
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.IO.FileInfo[] PhotoInfos { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            PhotoInfos = (from f in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "*.jpg")
                          select new System.IO.FileInfo(f)).ToArray();

            loadingUserControl.LoadingCompleted += delegate
            {
                gridRoot.Children.RemoveAt(gridRoot.Children.Count - 1);
                LibraryBarViewUserControl view = new LibraryBarViewUserControl();
                view.surfaceScatterViewer.ItemsSource = PhotoInfos.ToList();
                gridRoot.Children.Add(view);

                this.Topmost = true;
                this.Topmost = false;
            };

        }

    }
}
