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
        private const string TextoImage = "imagemTexto";

        public OCR()
        {
            SalvarImagemLocalAsync("https://i.pinimg.com/originals/d0/ca/2a/d0ca2aa755d2cfc9c3a7c57112be0bdc.jpg");
        }

        public void ExecuteOcr(string url)
        {
            Console.WriteLine($"Executando comando no CMD {ObterImagem()} ");
            // Console.WriteLine("echo oi".Bash());

            // Process process = new Process();
            // ProcessStartInfo startInfo = new ProcessStartInfo();
            // startInfo.UseShellExecute = false;
            // startInfo.RedirectStandardOutput = true;
            // startInfo.FileName = "cmd.exe";
            // process.EnableRaisingEvents = true;
            // process.Exited += p_Exited;
            // string comando = $"tesseract {ObterImagem()} {TextoImage} -l por";
            // Console.WriteLine(comando);
            // startInfo.Arguments = $"-c {comando}";

            // // startInfo.Arguments = "/c docker exec -it goocr go run main.go http://bonstutoriais.com.br/wp-content/uploads/2014/05/cionverter-texto-de-imagem-850x478.jpg";
            // process.StartInfo = startInfo;
            // process.Start();
            // string output = process.StandardOutput.ReadToEnd();
            // Console.WriteLine(output);
            // process.WaitForExit();
            Console.WriteLine("Commando finalizado ");
        }

        public void p_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine("Terminou o comando");
            // RemoverImagem();
        }

        private async Task SalvarImagemLocalAsync(string url)
        {
            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(url);
            File.WriteAllBytes(NomeImage, data);
        }

        private void RemoverImagem()
        {
             File.Delete(NomeImage);
        }
        private string ObterImagem()
        {
            return Path.GetFileName(NomeImage);
        }

    }
}
