using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace CHelpers
{
    public class WindowHandleHelper
    {
        #region 获取当前活动窗口的句柄
        /// <summary>
        /// 获取当前窗口句柄
        /// </summary>
        /// <returns>当前获得焦点窗口的句柄</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow">0 关闭窗口,1 正常大小显示窗口,2 最小化窗口,3 最大化窗口</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 获取窗口大小及位置
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        #region 获取指定进程句柄

        //internal const uint GW_OWNER = 4;

        //internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //internal static extern bool IsWindowVisible(IntPtr hWnd);

        ///// <summary>
        ///// 根据进程号获取进程主窗体句柄
        ///// </summary>
        ///// <param name="processId"></param>
        ///// <returns></returns>
        //public static IntPtr GetWindowHandle(int processId)
        //{
        //    IntPtr handle = IntPtr.Zero;

        //    EnumWindows(new EnumWindowsProc((hWnd, lParam) =>
        //    {
        //        IntPtr PID;

        //        GetWindowThreadProcessId(hWnd, out PID);

        //        if (PID == lParam && IsWindowVisible(hWnd) && GetWindow(hWnd, GW_OWNER) == IntPtr.Zero)
        //        {
        //            handle = hWnd;
        //            return false;
        //        }

        //        return true;
        //    }), new IntPtr(processId));

        //    return handle;
        //}

        #endregion

        #region C#获取进程的主窗口句柄

        //private static Hashtable processWnd = null;

        //public delegate bool WNDENUMPROC(IntPtr hWnd, uint lParam);

        //static WindowHandleHelper()
        //{
        //    if (processWnd == null)
        //    {
        //        processWnd = new Hashtable();
        //    }
        //}

        //[DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        //public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        //[DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        //public static extern IntPtr GetParent(IntPtr hWnd);

        //[DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        //public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        //[DllImport("user32.dll", EntryPoint = "IsWindow", SetLastError = true)]
        //public static extern bool IsWindow(IntPtr hWnd);

        //[DllImport("user32.dll", EntryPoint = "SetLastError", SetLastError = true)]
        //public static extern void SetLastError(uint dwErrorCode);

        //private static bool EnumWindowsProc(IntPtr hWnd, uint lParam)
        //{
        //    uint pid = 0;
        //    if (GetParent(hWnd) == IntPtr.Zero)
        //    {
        //        GetWindowThreadProcessId(hWnd, ref pid);
        //        if (pid == lParam)
        //        {
        //            processWnd[pid] = hWnd;
        //            SetLastError(0);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public static IntPtr GetCurrentWindowHandle(uint processId)
        //{
        //    IntPtr ptrWnd = IntPtr.Zero;

        //    object wnd = processWnd[processId];

        //    if (wnd != null)
        //    {
        //        ptrWnd = (IntPtr)wnd;
        //        if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))
        //        {
        //            return ptrWnd;
        //        }
        //        else
        //        {
        //            ptrWnd = IntPtr.Zero;
        //        }
        //    }

        //    bool result = EnumWindows(new WNDENUMPROC(EnumWindowsProc), processId);
        //    if (!result && Marshal.GetLastWin32Error() == 0)
        //    {
        //        wnd = processWnd[processId];
        //        if (wnd != null)
        //        {
        //            ptrWnd = (IntPtr)wnd;
        //        }
        //    }

        //    return ptrWnd;
        //}

        #endregion

        [DllImport("user32.dll")]
        public static extern int FindWindow(string strclassName, string strWindowName);

        

    }

    public class MyProcess
    {
        #region 微软实现的获取进程主窗口句柄代码
        private bool hasMainWindow = false;
        private IntPtr mainWindowHandle = IntPtr.Zero;
        private int processId = 0;

        public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumWindows(EnumThreadWindowsCallback callback, IntPtr extraData);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(HandleRef hWnd);

        public  IntPtr GetMainWindowHandle(int processId)
        {
            if (!this.hasMainWindow)
            {
                this.mainWindowHandle = IntPtr.Zero;
                this.processId = processId;
                EnumThreadWindowsCallback callback = new EnumThreadWindowsCallback(EnumWindowsCallback);
                EnumWindows(callback, IntPtr.Zero);
                GC.KeepAlive(callback);

                this.hasMainWindow = true;
            }

            return this.mainWindowHandle;
        }

        private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
        {
            int num;
            GetWindowThreadProcessId(new HandleRef(this, handle), out num);
            if ((num == this.processId) && this.IsMainWindow(handle))
            {
                this.mainWindowHandle = handle;
                return false;
            }
            return true;
        }

        private bool IsMainWindow(IntPtr handle)
        {
            return (!(GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && IsWindowVisible(new HandleRef(this, handle)));
        }

        #endregion
    }
}
