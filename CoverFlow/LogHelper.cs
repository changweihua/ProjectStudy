using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CoverFlow
{
    public class LogHelper
    {
        /// <summary>
        /// 记录信息到文本文件中
        /// 文件名根据日期自动生成
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            //using (Stream stream = new FileStream("log.txt", FileMode.OpenOrCreate))
            //{
            //    using (TextWriter writer = new StreamWriter(stream,))
            //    {
            //        writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " => " + message);
            //    }
            //}

            using (TextWriter writer = new StreamWriter(DateTime.Now.ToString("yyyy-MM-dd") + "-log.txt", true))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " => " + message);
            }
        }

    }

    public sealed class DebugMessage
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 操作发生时间
        /// </summary>
        public string ActionTime { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string ActionMessage { get; set; }

        public override string ToString()
        {
            return string.Format("操作名称=>{0}, 操作时间=>{1}, 操作信息=>{2}", ActionName, ActionTime, ActionMessage);
        }
    }

}
