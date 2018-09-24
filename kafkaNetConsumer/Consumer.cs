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
        private static Consumer<Ignore, string> GetConsumer() => new Consumer<Ignore, string>(conf, null, new StringDeserializer(Encoding.UTF8));

        static void Main(string[] args)
        {
            conf = new Dictionary<string, object>
            {
            { "group.id", "Grupo" },
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
            ConsumirMensagemEspecifica(10);
        }

        private static void ConsumirFila()
        {
            using (var consumer = GetConsumer())
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
                    BuscarNovaMensagem(consumer);
            }
        }

        private static void BuscarNovaMensagem(Consumer<Ignore, string> consumer) =>
            consumer.Poll(TimeSpan.FromMilliseconds(10));

        private static void ImprimirMensagem(Message<Ignore, string> msg) =>
            Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

        public static void ConsumirMensagemEspecifica(long posicalInicial)
        {
            using (var consumer = GetConsumer())
            {
                consumer.Assign(ConsumirAPartirDaPosicao(posicalInicial));

                consumer.OnMessage += (_, msg) =>
                {
                    var err = consumer.CommitAsync().Result.Error;
                    if (!err)
                        Console.WriteLine($"OUTRO TO: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");
                };

                consumer.OnError += (_, error)
                    => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, error)
                    => Console.WriteLine($"Consume error: {error}");

                while (true)
                {
                    if (consumer.Consume(out Message<Ignore, string> msg, TimeSpan.FromSeconds(1)))
                        ImprimirMensagem(msg);
                }
            }
        }

        private static IEnumerable<TopicPartitionOffset> ConsumirAPartirDaPosicao(long posicalInicial)
        {
            var off = new Offset(posicalInicial);
            var topicos = new[] { new TopicPartitionOffset(topic, 0, off) };
            return topicos;
        }
    }

}