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
using System.Windows.Forms.Integration;
using System.Runtime.InteropServices;
using System.Data;

namespace FlashGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {

        Config cfg = ((App)Application.Current).CurrentConfig;

        public MainWindow()
        {
            InitializeComponent();

            if (cfg.RememberLocation)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = cfg.WindowLocation.X;
                this.Top = cfg.WindowLocation.Y;
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<GameInfo> games = new List<GameInfo>();
            string connString = string.Format(@"data source={0}\data\hezi.sl3", Environment.CurrentDirectory);
            
            //var ctx =new HeziContext(connString);

            //games = ctx.GetTable<GameInfo>().ToList();

           
            DataTable dt =  SQLiteHelper.ExecuteDataSet(connString, "select * from gameinfo where status = 1", null).Tables[0];

            if (dt.Rows.Count > 0)
            {
                GameUC guc = null;
                GameInfo game = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    guc = new GameUC();
                    game = new GameInfo
                    {
                        Id = Convert.ToInt32(dt.Rows[i]["id"]),
                        Name = dt.Rows[i]["name"].ToString(),
                        XName = (dt.Rows[i]["xname"] == DBNull.Value) ? "" : dt.Rows[i]["xname"].ToString(),
                        Cover = (dt.Rows[i]["xname"] == DBNull.Value) ? "" : dt.Rows[i]["cover"].ToString(),
                        Description = (dt.Rows[i]["description"] == DBNull.Value) ? "" : dt.Rows[i]["description"].ToString()
                    };
                    guc.DataContext = game;
                    mainPanel.Children.Add(guc);
                }

            }
            
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.Owner = this;
            aw.ShowDialog();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow sw = new SettingWindow();
            sw.Owner = this;
            sw.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //NLog.LogManager.GetCurrentClassLogger().Info(string.Format("({0}, {1})", this.Left, this.Top));

            //判断是否需要记住窗体位置
           

            if(cfg.RememberLocation)
            {
                cfg.WindowLocation = new Point(this.Left, this.Top);
                SerializeHelper.Serialize(SerializeType.Binary, cfg, string.Format(@"{0}\config.dat", ((App)Application.Current).CurrentDirectory));
                ((App)Application.Current).CurrentConfig = cfg;

            }
        }

        private void PortraitButton_Click(object sender, RoutedEventArgs e)
        {
            ProtraitWindow pw = new ProtraitWindow();
            pw.Owner = this;
            pw.ShowDialog();
        }

       

    }

    
}
