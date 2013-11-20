using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FlashGame
{
    [Serializable]
    public class Config
    {
        //是否开机启动
        public bool AutoRun { get; set; }

        //是否记住主窗体位置
        public bool RememberLocation { get; set; }

        //主窗体位置
        public Point WindowLocation { get; set; }

        //是否是深色主题
        public bool IsDarkTheme { get; set; }

        //自定义游戏保存位置
        public string SwfDirectory { get; set; }

        //默认游戏保存位置
        public string DefaultSwfDirectory { get; set; }

        //是否静音
        public bool IsSilent { get; set; }

        //音量
        public int Volume { get; set; }
    }

    [Serializable]
    public class DefaultConfig
    {
        //是否开机启动
        public bool AutoRun = false;

        //是否记住主窗体位置
        public bool RememberLocation = false;

        //主窗体位置
        public Point WindowLocation = new Point(-1, -1);

        //是否是深色主题
        public bool IsDarkTheme = true;

        //自定义游戏保存位置
        public string SwfDirectory = Environment.CurrentDirectory + @"\\game";

        //是否静音
        public bool IsSilent = false;

        //音量
        public int Volume = 10;
    }
}
