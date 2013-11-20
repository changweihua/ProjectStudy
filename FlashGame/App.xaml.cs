using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace FlashGame
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private string currentDirectory = Environment.CurrentDirectory;

        public string CurrentDirectory
        { get { return currentDirectory; } }

        private Config config = new Config();

        public Config CurrentConfig
        {
            get { return config; }
            set { config = value; }
        }

        private void StartupHandler(object sender, System.Windows.StartupEventArgs e)
        {
            Elysium.Manager.Apply(this, Elysium.Theme.Dark, Elysium.AccentBrushes.Blue, Elysium.AccentBrushes.Lime);
        }
    }
}
