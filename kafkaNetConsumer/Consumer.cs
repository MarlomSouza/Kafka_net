using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace kafkaNetConsumer
{
    class Consumer
    {
        static Dictionary<string, object> conf;

        static void Main(string[] args)
        {
            conf = new Dictionary<string, object>
            {
            { "group.id", "test-consumer-group" },
            { "bootstrap.servers", "localhost:9092" },
            { "auto.offset.reset", "earliest" }
            };
            Consume();
            // ExecutarOCR();
        }

        private static void Consume()
        {
            var topic = "IDGTestTopic";

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {

                consumer.OnMessage += (_, msg) =>
                {
                    var err = consumer.CommitAsync().Result.Error;
                    if (!err)
                        OCR(msg);
                };

                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe(topic);
                while (true)
                {
                    BuscarNovaMensagem(consumer);
                    Console.WriteLine("Puxando nova mensagem......");
                }
            }
        }

        private static void BuscarNovaMensagem(Consumer<Null, string> consumer)
        {
            ExecutarOCR();
            consumer.Poll(TimeSpan.FromMilliseconds(10));

        }

        private static void ExecutarOCR()
        {
            Console.Write("Executando comando no CMD ");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            process.EnableRaisingEvents = true;
            process.Exited += p_Exited;

            startInfo.Arguments = "/c  timeout /t 10 && echo 0oooooi";
            // startInfo.Arguments = "/c docker exec -it goocr go run main.go http://bonstutoriais.com.br/wp-content/uploads/2014/05/cionverter-texto-de-imagem-850x478.jpg";
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            process.WaitForExit();
            Console.WriteLine("Commando finalizado ");
        }
        public static void p_Exited(object sender, EventArgs e)
        {
            Console.WriteLine("Enviou para fila de processados");
        }
        private static void OCR(Message<Null, string> msg)
        {
            Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
        }



    }

}