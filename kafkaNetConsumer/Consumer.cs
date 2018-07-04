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

           
            Console.WriteLine("Commando finalizado ");
        }
        private static void OCR(Message<Null, string> msg)
        {
            Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
        }



    }

}