using System.Net;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Diagnostics;

namespace OcrApi
{
    public class Ocr
    {
        private const string NomeImage = "imagemTexto.jpg";
        private const string TextoImage = "imagemTexto";
        public async Task ExecuteOcrAsync(string url, int quantidade)
        {
            await SalvarImagemLocalAsync(url);
            Console.WriteLine($"Executando comando no CMD {quantidade}");

            ExecuteTesseract(quantidade);

            Console.WriteLine($"Commando finalizado {quantidade}");
        }

        private void ExecuteTesseract(int quantidade)
        {
            // string comando = $"-c \"tesseract {ObterImagem()} {TextoImage + quantidade} -l por\"";
            string comando = $"/c timeout /t 10";
            ExecuteTesseract(comando);
        }
        private void ExecuteTesseract(string comando)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            // startInfo.FileName = "/bin/bash";
            process.EnableRaisingEvents = true;
            process.Exited += p_Exited;
            Console.WriteLine(comando);
            startInfo.Arguments = $"{comando}";
            // startInfo.Arguments = "/c docker exec -it goocr go run main.go http://bonstutoriais.com.br/wp-content/uploads/2014/05/cionverter-texto-de-imagem-850x478.jpg";
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();
        }

        public void p_Exited(object sender, System.EventArgs e)
        {
            Console.WriteLine("Terminou o comando");
            // RemoverImagem();
        }
        public async Task SalvarImagemLocalAsync(string url)
        {

            using (var httpClient = new HttpClient())
            using (var contentStream = await httpClient.GetStreamAsync(url))
            using (var fileStream = new FileStream(NomeImage, FileMode.Create, FileAccess.Write, FileShare.None, 1048576, true))
            {
                await contentStream.CopyToAsync(fileStream);
            }

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