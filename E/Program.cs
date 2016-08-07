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
        static Process Exec(string proc, string args) => 
            Process.Start(new ProcessStartInfo
                {
                    FileName = proc,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    Arguments = args
                });

        static Process Emacs(string args) => Exec("emacsclient", $"-n -a \"runemacs -mm\" {args}");

        static void Focus()
        {
            const string focus = "(select-frame-set-input-focus (selected-frame))";
            const string maximize = "(set-frame-parameter nil 'fullscreen 'maximized)";

            Emacs($"-e \"{focus}\"");
            Emacs($"-e \"{maximize}\"");
        }

        static void Main(string[] args)
        {
            var arguments = (args.Any())
                ? string.Join(" ", args)
                : "-c";

            if (Process.GetProcessesByName("emacs").Any())
            {
                if (args.Any())
                    Emacs(arguments);
                Focus();
            }
            else
            {
                Exec("runemacs", "-mm " + string.Join(" ", args));
            }
        }
    }
}
