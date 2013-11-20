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

namespace FlashGame
{
    /// <summary>
    /// GameUC.xaml 的交互逻辑
    /// </summary>
    public partial class GameUC : UserControl
    {
        Config cfg = ((App)System.Windows.Application.Current).CurrentConfig;

        public GameUC()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var windows = App.Current.Windows;
            GameWindow gw = null;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].GetType() == typeof(GameWindow) && windows[i].Tag.ToString() == this.Tag.ToString())
                {
                    gw = windows[i] as GameWindow;
                }
            }

            if (gw == null)
            {
                gw = new GameWindow(cfg.SwfDirectory + @"\game\" + tbName.Tag.ToString()); ;
                //(this.DataContext as GameInfo).Name;
                gw.DataContext = this.DataContext ;
                gw.Show();
            }
            else
            {
                gw.Activate();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow cw = new ConfirmWindow();
            //cw.Owner = ((this.Parent as WrapPanel).Parent as Decorator).Parent as Window;
            cw.Owner = App.Current.MainWindow;
            bool? result = cw.ShowDialog();

            if(result == true)
            {
                //System.IO.File.Delete("");
            }

        }
    }
}
