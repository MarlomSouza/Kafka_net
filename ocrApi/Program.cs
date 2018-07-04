using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ocrApi
{
    class Program
    {
        static void Main(string[] args)
        {
            new OCR().ExecuteOcr("");
        }
    }

    class OCR
    {
        private const string NomeImage = "imagemTexto.jpg";
        private const string TextoImage = "imagemTexto.txt";

        public void ExecuteOcr(string url)
        {
            Console.Write("Executando comando no CMD ");
            Console.WriteLine("echo oi".Bash());

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "sh";
            process.EnableRaisingEvents = true;
            // process.Exited += p_Exited;

            startInfo.Arguments = $"-c tesseract {ObterImagem()} {TextoImage} -l por";

            // startInfo.Arguments = "/c docker exec -it goocr go run main.go http://bonstutoriais.com.br/wp-content/uploads/2014/05/cionverter-texto-de-imagem-850x478.jpg";
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();
            Console.WriteLine("Commando finalizado ");
        }

        private void SalvarImagemLocal(string url) => File.WriteAllBytes(NomeImage, new WebClient().DownloadData(url));

        private string ObterImagem() => Path.GetTempFileName(NomeImage);


        // public void p_Exited(object sender, EventArgs e)
        // {
        //     Console.WriteLine("Enviou para fila de processados");
        // }

    }
}
