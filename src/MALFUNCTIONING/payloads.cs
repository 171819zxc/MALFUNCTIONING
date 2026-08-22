using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MALFUNCTIONING.api;
using static MALFUNCTIONING.Variables;
using static MALFUNCTIONING.function;
using static MALFUNCTIONING.Sounds;
using static MALFUNCTIONING.execute;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MALFUNCTIONING
{
    internal class payloads
    {
        public static void ShowWarning()
        {
            var message1 = $@"Pre-run warning - MALFUNCTIONING.EXE ！ Warning!

This program will cause your computer to temporarily lose control.
By running this program, your computer will experience various screen flickering phenomena and emit various sounds.

This program is only for learning and entertainment purposes. Do not use it to damage your computer.
All problems arising from executing this program will be the responsibility of the user (including legal liability).
This program allows modification. The modified program also has the user bear (including legal liability).

This program does not have the function of damaging the system (i.e., it does not have obvious malicious functions such as deleting files, damaging the MBR, modifying the registry, etc., which are permanent malicious functions). It requires caution during execution.
If you are not sure about this program, please exit immediately. This is the best choice.

If you want to study this program, it is best to run it in a virtual machine. It is recommended that the memory be ≥ 4GB and the CPU be ≥ 2 cores.
It is recommended to restart the computer after the operation is completed.
Before running this program, please make sure to save the file first.


Do you still want to run it?
";
            var message2 = $@"!!!Final Warning!!! 
You are currently running in an unsafe mode, and a blue screen will occur! 
We strongly recommend that you save the file and then run it in the virtual machine.";
            if (MessageBox(IntPtr.Zero, message1, "Pre-run Warning - MALFUNCTIONING.EXE ", 0x4 | 0x30 | 0x100 | 0x1000 | 0x10000/*MB_YESNO|MB_ICONWARNING|MB_DEFBUTTON2|MB_SYSTEMMODAL|MB_SETFOREGROUND*/) == 0x7/*IDNO*/)
            {
                Environment.Exit(0);
            }
            if (UnsafeMode)
            {
                if (MessageBox(IntPtr.Zero, message2, "Final Warning!!! ", 0x4 | 0x30 | 0x100 | 0x1000 | 0x10000/*MB_YESNO|MB_ICONWARNING|MB_DEFBUTTON2|MB_SYSTEMMODAL|MB_SETFOREGROUND*/) == 0x7/*IDNO*/)
                {
                    Environment.Exit(0);
                }
            }
        }
        public static void DrawCur()
        {
            var cur = LoadCursor(IntPtr.Zero, 32512);
            while (true)
            {
                var hdc = GetWindowDC(dsk);
                DrawIcon(hdc, RandX(), RandY(), cur);
                ReleaseDC(dsk, hdc);
                Sleep(5);
            }
        }
        public static void ScreenCopy()
        {
            while (true)
            {
                var x = Randint(-3, 3);
                var y = Randint(-3, 3);
                var hdc = GetWindowDC(dsk);
                switch (Randint(0, 2))
                {
                    case 0:
                        BitBlt(hdc, x, y, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                        break;
                    case 1:
                        BitBlt(hdc, x, y, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCAND);
                        break;
                    case 2:
                        BitBlt(hdc, x, y, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCPAINT);
                        break;
                }
                BitBlt(hdc, -x, -y, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                ReleaseDC(dsk, hdc);
                GC.Collect();
                Sleep(2000);
            }
        }
        public static void GDI1()
        {
            while (payload == 0)
            {
                var hdc = GetDC(dsk);
                RedScreen();
                StretchBlt(hdc, RandX(), RandY(), Randint(0, 800), Randint(0, 600), hdc, RandX(), RandY(), Randint(0, 800), Randint(0, 600), TernaryRasterOperations.SRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(20);
            }
        }
        public static void GDI2()
        {
            var deviation = 100;
            var copywidth = 100;
            while (payload == 1)
            {
                var hdc = GetWindowDC(dsk);
                var hMemDC = CreateCompatibleDC(hdc);
                var hBitMap = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap = SelectObject(hMemDC, hBitMap);
                BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                var lppoints = new POINT[3]
                {
                    new POINT() {x = deviation, y = -deviation},
                    new POINT() {x = ScrWidth + deviation, y = deviation},
                    new POINT() {x = -deviation, y = ScrHeight - deviation}
                };
                PlgBlt(hdc, lppoints, hMemDC, -copywidth, -copywidth, ScrWidth + copywidth * 2, ScrHeight + copywidth * 2, IntPtr.Zero, 0, 0);
                SelectObject(hMemDC, hOldBitMap);
                DeleteObject(hBitMap);
                DeleteDC(hMemDC);
                var hBrush = CreateSolidBrush(RandColorLight());
                var hOldBrush = SelectObject(hdc, hBrush);
                PatBlt(hdc, 0, 0, ScrWidth, ScrHeight, TernaryRasterOperations.PATINVERT);
                SelectObject(hdc, hOldBrush);
                DeleteObject(hBrush);
                StretchBlt(hdc, RandX(), RandY(), 300, 300, hdc, 0, 0, ScrWidth, ScrHeight, TernaryRasterOperations.SRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(50);
            }
        }
        public static void GDI3()
        {
            while (payload == 2)
            {
                var hdc = GetWindowDC(dsk);
                var lpPloy = new POINT[Randint(1, 10)];
                for (int i = 0; i < lpPloy.Length; i++)
                {
                    lpPloy[i] = new POINT() { x = RandX(), y = RandY() };
                }
                var hRgn = CreatePolygonRgn(lpPloy, lpPloy.Length, 1);
                var hOldRgn = SelectClipRgn(hdc, hRgn);
                var Brush = CreateHatchBrush(Randint(0, 5), (int)RandColor());
                var hOldBrush = SelectObject(hdc, Brush);
                SetBkColor(hdc, (int)RandColor());
                Rectangle(hdc, 0, 0, ScrWidth, ScrHeight);
                SelectObject(hdc, hOldBrush);
                DeleteObject(Brush);
                SelectClipRgn(hdc, (IntPtr)hOldRgn);
                DeleteObject(hRgn);
                ReleaseDC(dsk, hdc);
                Sleep(50);
            }
        }
        public static void GDI4()
        {
            while (payload == 3)
            {
                var hdc = GetWindowDC(dsk);
                var hBrush = CreateSolidBrush(RandColor());
                var hOldBrush = SelectObject(hdc, hBrush);
                PatBlt(hdc, 0, 0, ScrWidth, ScrHeight, TernaryRasterOperations.PATINVERT);
                BitBlt(hdc, Randint(-10, 10), Randint(-10, 10), ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCAND);
                SelectObject(hdc, hOldBrush);
                DeleteObject(hBrush);
                ReleaseDC(dsk, hdc);
                Sleep(8);
            }
        }
        public static void GDI5()
        {
            var width = 3;
            var height = 2;
            while (payload == 4)
            {
                var hdc = GetWindowDC(dsk);
                var hMemDC = CreateCompatibleDC(hdc);
                var hBitMap = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap = SelectObject(hMemDC, hBitMap);
                BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                for (int y = 0; y <= ScrHeight; y += height)
                {
                    BitBlt(hMemDC, Randint(-width, width), y, ScrWidth, height, hMemDC, 0, y, TernaryRasterOperations.SRCCOPY);
                }
                BitBlt(hdc, 0, 0, ScrWidth, ScrHeight, hMemDC, 0, 0, TernaryRasterOperations.SRCCOPY);
                SelectObject(hMemDC, hOldBitMap);
                DeleteObject(hBitMap);
                DeleteDC(hMemDC);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static void GDI6()
        {
            var width = 150;
            while (payload == 5)
            {
                var x = RandX();
                var y = RandY();
                var hdc = GetWindowDC(dsk);
                var hRgn = CreateEllipticRgn(x, y, x + width, y + width);
                var hOldRgn = SelectClipRgn(hdc, hRgn);
                BitBlt(hdc, x, y, width, width, hdc, RandX(), RandY(), TernaryRasterOperations.SRCCOPY);
                SelectClipRgn(hdc, (IntPtr)hOldRgn);
                DeleteObject(hRgn);
                ReleaseDC(dsk, hdc);
                Sleep(30);
            }
        }
        public static void GDI7()
        {
            var width = 80;
            while (payload == 6)
            {
                var hdc = GetWindowDC(dsk);
                var hMemDC = CreateCompatibleDC(hdc);
                var hBitMap = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap = SelectObject(hMemDC, hBitMap);
                BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                BitBlt(hdc, width, 0, ScrWidth, ScrHeight, hMemDC, 0, 0, TernaryRasterOperations.SRCCOPY);
                BitBlt(hdc, -ScrWidth + width, 0, ScrWidth, ScrHeight, hMemDC, 0, 0, TernaryRasterOperations.SRCCOPY);
                SelectObject(hMemDC, hOldBitMap);
                DeleteObject(hBitMap);
                DeleteDC(hMemDC);
                var hIcon = LoadIcon(IntPtr.Zero, Randint(32512, 32516));
                for (int _ = 0; _ < 3; _++) DrawIcon(hdc, RandX(), RandY(), hIcon);
                ReleaseDC(dsk, hdc);
                Sleep(100);
            }
        }
        public static void GDI8()
        {
            while (payload == 7)
            {
                var hdc = GetWindowDC(dsk);
                var lpPoly = new POINT[3]
                {
                    new POINT() { x = 0, y = 0},
                    new POINT() { x = 0, y = ScrHeight},
                    new POINT() { x = ScrWidth, y = ScrHeight}
                };
                var Rgn = CreatePolygonRgn(lpPoly, lpPoly.Length, 1);
                var hOldRgn = SelectClipRgn(hdc, Rgn);
                var hMemDC = CreateCompatibleDC(hdc);
                var hBitMap = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap = SelectObject(hMemDC, hBitMap);
                BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                _BLENDFUNCTION bLENDFUNCTION;
                bLENDFUNCTION.BlendOp = 0;
                bLENDFUNCTION.BlendFlags = 0;
                bLENDFUNCTION.SourceConstantAlpha = 200;
                bLENDFUNCTION.AlphaFormat = 1;
                AlphaBlend(hdc, 0, 0, ScrWidthHalf, ScrHeightHalf, hMemDC, 0, 0, ScrWidth, ScrHeight, bLENDFUNCTION);
                AlphaBlend(hdc, 0, ScrHeightHalf, ScrWidthHalf, ScrHeightHalf, hMemDC, 0, 0, ScrWidth, ScrHeight, bLENDFUNCTION);
                AlphaBlend(hdc, ScrWidthHalf, ScrHeightHalf, ScrWidthHalf, ScrHeightHalf, hMemDC, 0, 0, ScrWidth, ScrHeight, bLENDFUNCTION);
                SelectObject(hMemDC, hOldBitMap);
                DeleteObject(hBitMap);
                DeleteDC(hMemDC);
                SelectClipRgn(hdc, (IntPtr)hOldRgn);
                DeleteObject(Rgn);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static void GDI9()
        {
            var width = 10;
            while (payload == 8)
            {
                var hdc = GetWindowDC(dsk);
                var hMemDC = CreateCompatibleDC(hdc);
                var hBitMap = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap = SelectObject(hMemDC, hBitMap);
                BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                var lppoint = new POINT[3]
                {
                    new POINT() { x = -width, y = 0},
                    new POINT() { x = ScrWidth - width, y = 0},
                    new POINT() { x = width, y = ScrHeight}
                };
                PlgBlt(hdc, lppoint, hMemDC, 0, 0, ScrWidth, ScrHeight, IntPtr.Zero, 0, 0);
                SelectObject(hMemDC, hOldBitMap);
                DeleteObject(hBitMap);
                DeleteDC(hMemDC);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static void GDI10()
        {
            while (payload == 9)
            {
                var hdc = GetWindowDC(dsk);
                BitBlt(hdc, Randint(-5, 5), Randint(-5, 5), ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.NOTSRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static void GDI11()
        {
            var width = 10;
            while (payload == 10)
            {
                var hdc = GetWindowDC(dsk);
                var x = RandX();
                BitBlt(hdc, x, Randint(0, 10), width, ScrHeight, hdc, x, 0, TernaryRasterOperations.SRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static unsafe void GDI12()
        {
            while (payload == 11)
            {
                IntPtr hdcDesktop = IntPtr.Zero;
                IntPtr hMemDC = IntPtr.Zero;
                IntPtr hDib = IntPtr.Zero;
                IntPtr hOldObj = IntPtr.Zero;
                IntPtr pPixelBits = IntPtr.Zero;
                try
                {
                    hdcDesktop = GetWindowDC(dsk);
                    if (hdcDesktop == IntPtr.Zero) continue;
                    hMemDC = CreateCompatibleDC(hdcDesktop);
                    BITMAPINFO bmi = new BITMAPINFO();
                    bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
                    bmi.bmiHeader.biWidth = ScrWidth;
                    bmi.bmiHeader.biHeight = -ScrHeight; // 从上到下
                    bmi.bmiHeader.biPlanes = 1;
                    bmi.bmiHeader.biBitCount = 32;
                    bmi.bmiHeader.biCompression = 0;
                    bmi.bmiHeader.biSizeImage = 0;
                    bmi.bmiHeader.biXPelsPerMeter = 0;
                    bmi.bmiHeader.biYPelsPerMeter = 0;
                    bmi.bmiHeader.biClrUsed = 0;
                    bmi.bmiHeader.biClrImportant = 0;
                    hDib = CreateDIBSection(IntPtr.Zero, ref bmi, 0, out pPixelBits, IntPtr.Zero, 0);
                    if (hDib == IntPtr.Zero) continue;
                    hOldObj = SelectObject(hMemDC, hDib);
                    BitBlt(hMemDC, 0, 0, ScrWidth, ScrHeight, hdcDesktop, 0, 0, TernaryRasterOperations.SRCCOPY);

                    byte* pBuf = (byte*)pPixelBits.ToPointer();

                    int stride = ScrWidth * 4;
                    for (int y = 0; y < ScrHeight; y++)
                    {
                        for (int x = 0; x < ScrWidth; x++)
                        {
                            int pos = y * stride + x * 4;
                            byte B = pBuf[pos + 0];
                            byte G = pBuf[pos + 1];
                            byte R = pBuf[pos + 2];
                            byte gray = (byte)((R + G + B) / 3);

                            pBuf[pos + 0] = gray; // B
                            pBuf[pos + 1] = gray; // G
                            pBuf[pos + 2] = gray; // R
                        }
                    }

                    BitBlt(hdcDesktop, 0, 0, ScrWidth, ScrHeight, hMemDC, 0, 0, TernaryRasterOperations.SRCCOPY);
                }
                finally
                {
                    // finally保证GDI资源一定释放，防止GDI泄漏
                    if (hOldObj != IntPtr.Zero && hMemDC != IntPtr.Zero)
                        SelectObject(hMemDC, hOldObj);
                    if (hDib != IntPtr.Zero) DeleteObject(hDib);
                    if (hMemDC != IntPtr.Zero) DeleteDC(hMemDC);
                    if (hdcDesktop != IntPtr.Zero) ReleaseDC(dsk, hdcDesktop);
                }

                Sleep(80);
            }
        }
        public static void GDI13()
        {
            var width = 10;
            while (payload == 12)
            {
                var hdc = GetWindowDC(dsk);
                var hMemDC1 = CreateCompatibleDC(hdc);
                var hMemDC2 = CreateCompatibleDC(hdc);
                var hBitMap1 = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hBitMap2 = CreateCompatibleBitmap(hdc, ScrWidth, ScrHeight);
                var hOldBitMap1 = SelectObject(hMemDC1, hBitMap1);
                var hOldBitMap2 = SelectObject(hMemDC2, hBitMap2);
                BitBlt(hMemDC1, 0, 0, ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                List<int> xCoordinate = new List<int>() { 0};
                for (var i = 0;i < ScrWidth;i += width)
                {
                    xCoordinate.Insert(Randint(0, xCoordinate.Count - 1), i);
                }
                var count = 0;
                for (var i = 0; i < ScrWidth; i += width)
                {
                    BitBlt(hMemDC2, xCoordinate[count], 0, width, ScrHeight, hMemDC1, i, 0, TernaryRasterOperations.SRCCOPY);
                    count++; 
                    if (count == xCoordinate.Count) count = 0;
                }
                BitBlt(hdc, 0, 0, ScrWidth, ScrHeight, hMemDC2, 0, 0, TernaryRasterOperations.SRCCOPY);
                SelectObject(hMemDC1, hOldBitMap1);
                DeleteObject(hBitMap1);
                DeleteDC(hMemDC1);
                SelectObject(hMemDC2, hOldBitMap2);
                DeleteObject(hBitMap2);
                DeleteDC(hMemDC2);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        } 
    }
}
