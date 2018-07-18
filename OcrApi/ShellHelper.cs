using System.Diagnostics;

namespace ocrApi
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            // process.Exited += p_Exited;
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
            // }
            // public static void p_Exited(object sender, EventArgs e)
            // {
            //     Console.WriteLine("Enviou para fila de processados");
            // }

        }
    }
}