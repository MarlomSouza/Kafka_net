using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace kafkaNetProducer
{
    class Produce
    {
        private const string bootstrapServer = "bootstrap.servers";
        private readonly string uri;
        private readonly string topic;

        public Produce()
        {
            uri = "localhost:9092";
            topic = "IDGTestTopic";
            ProduzirMensagem();
        }

        static void Main(string[] args) => new Produce();

        private void ProduzirMensagem()
        {

            var config = new Dictionary<string, object> { { bootstrapServer, uri } };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                EnviarMensagem(producer);
                EnviarMensagem(producer);
                EnviarMensagem(producer);
                EnviarMensagem(producer);
            }

        }

        private void EnviarMensagem(Producer<Null, string> producer)
        {
            var dr = producer.ProduceAsync(topic, null, "test message text").Result;
            Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
        }

        public bool EnviarMensagem()
        {
            Console.WriteLine("Deseja enviar mais mensagem? ");
            return Console.ReadLine().Contains("s");
        }

    }
}
