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
        static Dictionary<string, object> conf = new Dictionary<string, object>
    {
      { "group.id", "test-consumer-group" },
      { "bootstrap.servers", "localhost:9092" },
      { "auto.commit.interval.ms", 5000 },
      { "auto.offset.reset", "earliest" }
    };

        static void Main(string[] args)
        {
            // Consume();
            ExecutarOCR();
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
                    Console.Write(".");
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
            Console.WriteLine("Executando comando no CMD");

            String command = @"timeout /t 02 && echo 0oooooi";
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = command;
            Process cmd = Process.Start(cmdsi);
            cmd.EnableRaisingEvents = true;
            cmd.Exited += new EventHandler(p_Exited);
            cmd.WaitForExit();
            Console.WriteLine("Commando finalizado");
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