using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MALFUNCTIONING
{
    internal class Variables
    {
        // -------------------------------
        public const int DefaultTime = 20;
        public const int DefaultLatentperiodTime = 5000;
        public static readonly string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        // -------------------------------
        public static IntPtr dsk = api.GetDesktopWindow();
        public static Random random = new Random();
        public static bool Warning = true;
        public static int payload = 0;
        public static int seconds = DefaultTime;
        public static bool UnsafeMode = false;
        public static int LatentperiodTime = DefaultLatentperiodTime;
        // -------------------------------
    }
}
