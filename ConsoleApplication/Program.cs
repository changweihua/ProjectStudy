using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Reflection;

using CHelpers;
using System.Diagnostics;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            #region List 集合合并

            //ListMegre.Test();

            #endregion

            #region Excel 合并

            //ExcelMegreTest.Test();
            
            #endregion

            //string year = "2016";
            
            //DateTime dt = new DateTime(Convert.ToInt32(year), 1, 1);

            //dt = Convert.ToDateTime("2016年");

            //dt = DateTime.Parse("2013/10/22 17:42:09");

            //Console.WriteLine(dt.ToString("yyyy/mm/dd"));

            //decimal d = 652142521100.22351m;
            //Console.WriteLine(d.ToString("###,###.#####"));

            //Console.WriteLine("9/2 = {0}", 9/2);

            //Process process = new Process();
            //process.StartInfo = new ProcessStartInfo(@"D:\GifCam.exe");
            //process.Start();
            
            ////IntPtr intPtr = WindowHandleHelper.GetForegroundWindow();
            ////IntPtr intPtr = (new MyProcess()).GetMainWindowHandle(Process.GetCurrentProcess().Id);
            //WindowHandleHelper.RECT rect = new WindowHandleHelper.RECT();
            //WindowHandleHelper.GetWindowRect(process.Handle, ref rect);
            //Console.WriteLine(process.ProcessName);
            //Console.WriteLine("({0}, {1}), {2}, {3}", rect.Top, rect.Left, rect.Right - rect.Left, rect.Bottom - rect.Top);

            Console.ReadKey(true);

        }


    }

}
