using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MALFUNCTIONING.api;
using static MALFUNCTIONING.Variables;

namespace MALFUNCTIONING
{
    internal class function
    {
        public static int Randint(int min, int max)
        {
            return random.Next(min, max + 1);
        }
        public static int ScrWidth = GetSystemMetrics(SM_CXSCREEN);
        public static int ScrHeight = GetSystemMetrics(SM_CYSCREEN);
        public static int ScrWidthHalf = ScrWidth / 2;
        public static int ScrHeightHalf = ScrHeight / 2;
        public static uint RGB(int red, int green, int blue)
        {
            return (uint)(red | (green << 8) | (blue << 16));
        }
        public static uint HSV(int hue)
        {
            float s = 1f;
            float v = 1f;
            float c = v * s;
            float x = c * (1 - Math.Abs((hue / 60f) % 2 - 1));

            float r = 0, g = 0, b = 0;
            if (hue < 60) { r = c; g = x; b = 0; }
            else if (hue < 120) { r = x; g = c; b = 0; }
            else if (hue < 180) { r = 0; g = c; b = x; }
            else if (hue < 240) { r = 0; g = x; b = c; }
            else if (hue < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }

            int R = (int)(r * 255f);
            int G = (int)(g * 255f);
            int B = (int)(b * 255f);

            return RGB(R, G, B);
        }
        public static Thread NewThread(ThreadStart start)
        {
            Thread newThread = new Thread(start);
            newThread.IsBackground = true;
            newThread.Start();
            return newThread;
        }
        public static int RotateDC(IntPtr hdc, int Angle, POINT ptCenter)
        {
            int nGraphicsMode = SetGraphicsMode(hdc, GM_ADVANCED);
            XFORM xform;
            if (Angle != 0)
            {
                double fangle = (double)Angle / 180 * 3.1415926;
                xform.eM11 = (float)Math.Cos(fangle);
                xform.eM12 = (float)Math.Sin(fangle);
                xform.eM21 = (float)-Math.Sin(fangle);
                xform.eM22 = (float)Math.Cos(fangle);
                xform.eDx = (float)(ptCenter.x - Math.Cos(fangle) * ptCenter.x + Math.Sin(fangle) * ptCenter.y);
                xform.eDy = (float)(ptCenter.y - Math.Cos(fangle) * ptCenter.y - Math.Sin(fangle) * ptCenter.x);
                SetWorldTransform(hdc, ref xform);
            }
            return nGraphicsMode;
        }
        public static uint RandColor()
        {
            return (uint)RGB(Randint(0,255), Randint(0,255), Randint(0,255));
        }
        public static uint RandColorLight()
        {
            return (uint)RGB(Randint(0, 127), Randint(0, 127), Randint(0, 127));
        }
        public static int RandX()
        {
            return Randint(0, ScrWidth);
        }
        public static int RandY()
        {
            return Randint(0, ScrHeight);
        }
		public static void Sleep(int ms)
		{
            Thread.Sleep(ms);
		}
        public static void RedScreen()
        {
            var hdc = GetWindowDC(dsk);
            var hBrush = CreateSolidBrush((uint)Randint(0,0xFF));
            var hOldBrush = SelectObject(hdc, hBrush);
            PatBlt(hdc, 0, 0, ScrWidth, ScrHeight, TernaryRasterOperations.PATINVERT);
            SelectObject(hdc, hOldBrush);
            DeleteObject(hBrush);
            ReleaseDC(dsk, hdc);
        }
    }
}
