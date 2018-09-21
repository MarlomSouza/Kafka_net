using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            { "group.id", Guid.NewGuid() },
            { "bootstrap.servers", "localhost:9092" },
            { "enable.auto.commit", true},
            {"enable.auto.offset.store", false},
            { "default.topic.config", new Dictionary<string, object>()
                {
                    { "auto.offset.reset", "earliest" }
                }
            }
            };
            // ConsumirFila();
            //consumir mensagem especifica
            ConsumirMensagemEspecifica();
            
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
            consumer.Poll(TimeSpan.FromMilliseconds(10));
        

        private static void ImprimirMensagem(Message<Null, string> msg) =>
            Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");

        private static void ConsumirMensagemEspecifica()
        {
            var topic = "IDGTestTopic";
            var topico = ConsumirDeOnde(topic);
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
                
                
                while (true)
                {
                    consumer.Assign(CriarTopicos(topic));
                }
            }
        }

        private static IEnumerable<TopicPartitionOffset> ConsumirDeOnde(string topic)
        {
            var offset = new Offset();
            var topico = new TopicPartitionOffset(topic, 0, offset);
            var topicos = new[] { topico };
            return topicos;
        }

        private static TopicPartition[] CriarTopicos(string topic)
        {
            TopicPartition partition0 = new TopicPartition(topic, 0);
            TopicPartition partition1 = new TopicPartition(topic, 1);
            var topicos = new[] { partition0, partition1 };
            return topicos;
        }
    }

}