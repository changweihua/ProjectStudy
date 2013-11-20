using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CoverFlow
{
    public class LogHelper
    {
        public static void Log(string message)
        {
            //using (Stream stream = new FileStream("log.txt", FileMode.OpenOrCreate))
            //{
            //    using (TextWriter writer = new StreamWriter(stream,))
            //    {
            //        writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " => " + message);
            //    }
            //}


            using (TextWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " => " + message);
            }
        }

    }
}
