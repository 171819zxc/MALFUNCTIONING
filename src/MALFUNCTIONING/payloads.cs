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

namespace MALFUNCTIONING
{
    internal class payloads
    {
        public static void ShowWarning()
        {
            var message = $@"Pre-run warning - MALFUNCTIONING.EXE ！ Warning!

This program will cause your computer to temporarily lose control.
By running this program, your computer will experience various screen flickering phenomena and emit various sounds.

This program is only for learning and entertainment purposes. Do not use it to damage your computer.
All problems arising from executing this program will be the responsibility of the user (including legal liability).
This program allows modification. The modified program also has the user bear (including legal liability).

This program does not have the function of damaging the system (i.e., it does not have obvious malicious functions such as deleting files, damaging the MBR, modifying the registry, etc., which are permanent malicious functions). It requires caution during execution.
If you are not sure about this program, please exit immediately. This is the best choice.

If you want to study this program, it is best to run it in a virtual machine. It is recommended that the memory be ≥ 4GB and the CPU be ≥ 2 cores.
It is recommended to restart the computer after the operation is completed.


Do you still want to run it?
";
            if (MessageBox(IntPtr.Zero, message, "Pre-run Warning - MALFUNCTIONING.EXE ", 0x4 | 0x30 | 0x100 | 0x1000 | 0x10000/*MB_YESNO|MB_ICONWARNING|MB_DEFBUTTON2|MB_SYSTEMMODAL|MB_SETFOREGROUND*/) == 0x7/*IDNO*/)
            {
                Environment.Exit(0);
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
                for (int y = 0;y <= ScrHeight; y += height)
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
                BitBlt(hdc, Randint(-5,5), Randint(-5,-5), ScrWidth, ScrHeight, hdc, 0, 0, TernaryRasterOperations.NOTSRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(10);
            }
        }
        public static void GDI11()
        {
            var x = 0;
            var width = 10;
            while (payload == 10)
            {
                var hdc = GetWindowDC(dsk);
                BitBlt(hdc, x, Randint(0, 10), width, ScrHeight, hdc, x, 0, TernaryRasterOperations.SRCCOPY);
                ReleaseDC(dsk, hdc);
                Sleep(10);
                x += width;
                if (x >= ScrWidth) x = 0;
            }
        }
    }
}
