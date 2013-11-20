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
using System.Threading;

namespace FlashGame
{
    /// <summary>
    /// InitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InitWindow : Elysium.Controls.Window
    {
        public InitWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((ThreadStart)delegate()
            {
                #region 配置文件

                Config config = null;

                //首先查看是否已经存在配置文件，如果不存在，创建默认配置文件，否则读取配置文件

                if (!System.IO.File.Exists(string.Format(@"{0}\config.dat", ((App)Application.Current).CurrentDirectory)))
                {

                    Config cfg = new Config
                    {
                        AutoRun = false,
                        IsDarkTheme = true,
                        IsSilent = false,
                        RememberLocation = false,
                        SwfDirectory = Environment.CurrentDirectory,
                        Volume = 10
                    };

                    SerializeHelper.Serialize(SerializeType.Binary, cfg, string.Format(@"{0}\config.dat", ((App)Application.Current).CurrentDirectory));

                    SerializeHelper.Serialize(SerializeType.Xml, cfg, string.Format(@"{0}\config.xml", ((App)Application.Current).CurrentDirectory));

                    NLog.LogManager.GetCurrentClassLogger().Info("创建默认配置文件");

                    config = cfg;

                }
                else
                {
                    config = (Config)SerializeHelper.Deserialize(SerializeType.Binary, typeof(Config), string.Format(@"{0}\config.dat", ((App)Application.Current).CurrentDirectory));
                    NLog.LogManager.GetCurrentClassLogger().Info("读取当前配置文件");


                }

                ((App)Application.Current).CurrentConfig = config;

                #endregion

                MainWindow mw = new MainWindow();
                mw.Show();
                App.Current.MainWindow = mw;
                this.Close();


            });

            
        }
    }
}
