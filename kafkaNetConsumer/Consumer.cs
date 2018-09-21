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
            { "auto.offset.reset", "earliest" },
            { "auto.commit.enable", false }
            };
            ConsumirFila();
        }

        private static void ConsumirFila()
        {
            var topic = "IDGTestTopic";

            using (var consumer = new Consumer<Null, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg) =>
                {
                    var err = consumer.CommitAsync().Result.Error;
                    if (!err)
                        ImprimirMensagem(msg);
                };

                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe(topic);
                while (true)
                {
                    BuscarNovaMensagem(consumer);
                }
            }
        }

        private static void BuscarNovaMensagem(Consumer<Null, string> consumer) =>
            consumer.Poll(TimeSpan.FromMilliseconds(100));
        

        private static void ImprimirMensagem(Message<Null, string> msg) =>
            Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
    }

}