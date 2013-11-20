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
using System.Data.SQLite;

namespace FlashGame
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Elysium.Controls.Window
    {

        Config cfg = ((App)Application.Current).CurrentConfig;

        public SettingWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.mainTab.SelectedIndex.ToString());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            GameFile.Text = ChooseFile("SWF动画文件|*.swf");
        }

        private string ChooseFile(string filter)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = filter;// "SWF动画文件|*.swf";
            var flag = ofd.ShowDialog();

            if ((bool)flag)
            {
                return  ofd.FileName;
            }

            return null;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;

            index = index == -1 ? 0 : index;

            index--;

            Double offset = 0;

            if(index !=-1)
            {
                offset = (spSetting.Children[index] as StackPanel).ActualHeight;
            }

            svSetting.ScrollToVerticalOffset(offset);
            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //将配置信息绑定到 DataContext 中
            this.spSetting.DataContext = ((App)Application.Current).CurrentConfig;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //保存所有配置项

            SerializeHelper.Serialize(SerializeType.Binary, cfg, string.Format(@"{0}\config.dat", ((App)Application.Current).CurrentDirectory));
            ((App)Application.Current).CurrentConfig = cfg;

            this.Close();
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            string connString = string.Format(@"data source={0}\data\hezi.sl3", Environment.CurrentDirectory);
            //SQLiteParameter p1 = new SQLiteParameter("@name", GameName.Text);
            //SQLiteParameter p2 = new SQLiteParameter("@xname", GameXName.Text);
            ////SQLiteParameter p3 = new SQLiteParameter("@description", GameName.Text);
            //SQLiteParameter p4 = new SQLiteParameter("@cover", GameXName.Text + ".jpg");
            int count = SQLiteHelper.ExecuteNonQuery(connString, string.Format("insert into gameinfo (name, xname, description, cover) values ('{0}', '{1}', '暂缺', '{2}') ", GameName.Text, GameXName.Text + ".swf", GameXName.Text + ".jpg"), null);

            System.IO.File.Copy(@GameFile.Text, cfg.SwfDirectory + @"\game\" + GameXName.Text + ".swf");
            System.IO.File.Copy(@GameCover.Text, cfg.SwfDirectory + @"\game\cover\" + GameXName.Text + ".jpg");

            NLog.LogManager.GetCurrentClassLogger().Debug(count);

        }

        private void ChooseCoverButton_Click(object sender, RoutedEventArgs e)
        {
            //GameCover.Text = ChooseFile("JPG,JEPG|*.jpg;*.jpeg|PNG|*.png");
            GameCover.Text = ChooseFile("JPG|*.jpg");
        }

    }
}
