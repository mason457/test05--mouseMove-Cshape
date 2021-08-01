using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Qmouse_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static Color GetPixelColor(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Color color = GetPixelColor(Cursor.Position);
            int x = 42, y = 290, v = 54;
            int[] mapx = new int[] { };
            int[] mapy = new int[] { };
            int[] x1 = new int[] { 1, 2, 2, 3, 3, 3, 2, 1, 1, 2, 3, 4, 5, 6, 6, 5, 5, 4, 5, 5, 6, 6, 7 };
            int[] y1 = new int[] { 0, 0, 1, 1, 2, 3, 3, 3, 4, 4, 4, 4, 4, 4, 3, 3, 2, 2, 2, 3, 3, 4, 4 };
            int[] x2 = new int[] { 0, 1, 1, 2, 2, 2, 1, 2, 2, 2, 3, 4, 5, 6, 5, 4, 3, 2, 2, 2, 3, 4, 4, 5, 6, 6, 7, 7, 7, 7 };
            int[] y2 = new int[] { 1, 1, 0, 0, 1, 2, 2, 2, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 2, 2, 2, 1, 1, 1, 0, 0, 1, 2, 3 };
            int[] x3 = new int[] { 1, 2, 2, 3, 4, 5, 5, 6, 7, 7, 7, 6, 5, 5, 4, 3, 2, 2, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 3, 4, 4, 5, 6, 6, 7 };
            int[] y3 = new int[] { 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 2, 2, 2, 1, 1, 1, 1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 3, 2, 2, 3, 3, 3, 4, 4 };
            if (v > 0)
            {
                mapx = x3;
                mapy = y3;
                for (int i = 0; i < mapx.Length; i++)
                {
                    if (color.R < 200)
                    {
                        Thread.Sleep(3000);
                        Mouse.MoveTo(67, 506);
                        Mouse.LeftClick();
                    }
                    Thread.Sleep(1500);
                    Mouse.MoveTo(x + mapx[i] * v, y + mapy[i] * v);
                    Mouse.LeftClick();
                }
                Thread.Sleep(1500);
                Mouse.MoveTo(430, 490);
                Mouse.LeftClick();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Start(); 
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Color color = GetPixelColor(Cursor.Position);
            this.Text = "(" + Cursor.Position.X.ToString() + "," + Cursor.Position.Y.ToString() + ")";
            label1.Text = " R = " + color.R;
            label2.Text = " G = " + color.G;
            label3.Text = " B = " + color.B;
        }
    }

    static public class Mouse
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 SendInput(Int32 cInputs, ref INPUT pInputs, Int32 cbSize);
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public INPUTTYPE dwType;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MOUSEINPUT
        {
            public Int32 dx;
            public Int32 dy;
            public Int32 mouseData;
            public MOUSEFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct KEYBOARDINPUT
        {
            public Int16 wVk;
            public Int16 wScan;
            public KEYBOARDFLAG dwFlags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HARDWAREINPUT
        {
            public Int32 uMsg;
            public Int16 wParamL;
            public Int16 wParamH;
        }

        public enum INPUTTYPE : int
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags()]
        public enum MOUSEFLAG : int
        {
            MOVE = 0x1,
            LEFTDOWN = 0x2,
            LEFTUP = 0x4,
            RIGHTDOWN = 0x8,
            RIGHTUP = 0x10,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            XDOWN = 0x80,
            XUP = 0x100,
            VIRTUALDESK = 0x400,
            WHEEL = 0x800,
            ABSOLUTE = 0x8000
        }

        [Flags()]
        public enum KEYBOARDFLAG : int
        {
            EXTENDEDKEY = 1,
            KEYUP = 2,
            UNICODE = 4,
            SCANCODE = 8
        }

        static public void LeftDown()
        {
            INPUT leftdown = new INPUT();

            leftdown.dwType = 0;
            leftdown.mi = new MOUSEINPUT();
            leftdown.mi.dwExtraInfo = IntPtr.Zero;
            leftdown.mi.dx = 0;
            leftdown.mi.dy = 0;
            leftdown.mi.time = 0;
            leftdown.mi.mouseData = 0;
            leftdown.mi.dwFlags = MOUSEFLAG.LEFTDOWN;

            SendInput(1, ref leftdown, Marshal.SizeOf(typeof(INPUT)));
        }

        static public void LeftUp()
        {
            INPUT leftup = new INPUT();

            leftup.dwType = 0;
            leftup.mi = new MOUSEINPUT();
            leftup.mi.dwExtraInfo = IntPtr.Zero;
            leftup.mi.dx = 0;
            leftup.mi.dy = 0;
            leftup.mi.time = 0;
            leftup.mi.mouseData = 0;
            leftup.mi.dwFlags = MOUSEFLAG.LEFTUP;

            SendInput(1, ref leftup, Marshal.SizeOf(typeof(INPUT)));
        }

        static public void LeftClick()
        {
            LeftDown();
            Thread.Sleep(20);
            LeftUp();
        }

        static public void LeftDoubleClick()
        {
            LeftClick();
            Thread.Sleep(50);
            LeftClick();
        }

        /*static public void DragTo(string sor_X, string sor_Y, string des_X, string des_Y)
        {
            MoveTo(sor_X, sor_Y);
            LeftDown();
            Thread.Sleep(200);
            MoveTo(des_X, des_Y);
            LeftUp();
        }
        */
        static public void MoveTo(int tx, int ty)
        {
            int x, y;
            //int.TryParse(tx, out x);
            //int.TryParse(ty, out y);

            Cursor.Position = new Point(tx, ty);
        }
    }
}
