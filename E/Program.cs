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

        static Process Emacs(string args) => Exec("emacsclient", $"-n -a \"runemacs\" -F \"((fullscreen . maximized))\" \"{args}\"");
        static string Eval(string lisp) => Emacs($"-e \"{lisp}\"").StandardOutput.ReadToEnd();
        static bool Running() => Process.GetProcessesByName("emacs").Any();

        static void Main(string[] args)
        {
            var arguments = (args.Any())
                ? string.Join(" ", args)
                : "-c";

            if (Running())
            {
                if (args.Any())
                {
                    Emacs(arguments).StandardOutput.ReadToEnd();
                    Eval("(set-frame-parameter nil 'fullscreen 'maximized)");
                }
                else
                {
                    var frames = int.Parse(Eval("(length (visible-frame-list))"));

                    if (frames > 0)
                        Eval("(select-frame-set-input-focus (selected-frame))");
                    else
                        Emacs("-c");
                }
            }
            else
            {
                Exec("runemacs", "-mm " + string.Join(" ", args));
            }
        }
    }
}
