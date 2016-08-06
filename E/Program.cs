using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E
{
    class Program
    {
        static string Exec(string proc, string args)
        {
            var info = new ProcessStartInfo
            {
                FileName = proc,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = args
            };

            return Process.Start(info)
                .StandardOutput.ReadToEnd()
                .Trim();
        }

        static string Emacs(string args) => Exec("emacsclient", $"-n -a \"runemacs\" {args}");

        static void Focus()
        {
            const string focus = "(select-frame-set-input-focus (selected-frame))";
            const string maximize = "(set-frame-parameter nil 'fullscreen 'maximized)";

            Emacs($"-e \"(progn {focus} {maximize})\"");
        }

        static void Main(string[] args)
        {
            var arguments = (args.Any())
                ? string.Join(" ", args)
                : "-c";

            Emacs(arguments);
            Focus();
        }
    }
}
