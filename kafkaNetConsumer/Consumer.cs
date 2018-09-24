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
        private static string topic = "IDGTestTopic";

        static void Main(string[] args)
        {
            conf = new Dictionary<string, object>
            {
            { "group.id", Guid.NewGuid() },
            { "bootstrap.servers", "localhost:9092" },
            { "enable.auto.commit", false},
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
                    BuscarNovaMensagem(consumer);
                }
            }
        }

        private static void BuscarNovaMensagem(Consumer<Null, string> consumer) =>
            consumer.Poll(TimeSpan.FromMilliseconds(10));
        

        private static void ImprimirMensagem(Message<Null, string> msg) =>
            Console.WriteLine($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");

     public static void ConsumirMensagemEspecifica()
        {
            conf = new Dictionary<string, object>{
            { "group.id", new Guid().ToString() },
                { "bootstrap.servers", "localhost:9092" },
                // partition offsets can be committed to a group even by consumers not
                // subscribed to the group. in this example, auto commit is disabled
                // to prevent this from occuring.
                { "enable.auto.commit", false }
            };
            using (var consumer = new Consumer<Ignore, string>(conf, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Assign(ConsumirDeOnde());

                // Raised on critical errors, e.g. connection failures or all brokers down.
                consumer.OnError += (_, error)
                    => Console.WriteLine($"Error: {error}");

                // Raised on deserialization errors or when a consumed message has an error != NoError.
                consumer.OnConsumeError += (_, error)
                    => Console.WriteLine($"Consume error: {error}");

                while (true)
                {
                    if (consumer.Consume(out Message<Ignore, string> msg, TimeSpan.FromSeconds(1)))
                    {
                        Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");
                    }
                }
            }
        }

        private static IEnumerable<TopicPartitionOffset> ConsumirDeOnde()
        {
            // TopicPartitionOffset topico = ParticaoOffSet();
            var topicos = new[] { new TopicPartitionOffset(topic, 0, Offset.Beginning) };
            return topicos;
        }

        private static TopicPartitionOffset ParticaoOffSet()
        {
            var topico = new TopicPartitionOffset(topic, 0, Offset.Beginning);
            return topico;
        }

        private static TopicPartition[] CriarTopicos()
        {
            TopicPartition partition0 = new TopicPartition(topic, 0);
            var topicos = new[] { partition0 };
            return topicos;
        }
    }

}