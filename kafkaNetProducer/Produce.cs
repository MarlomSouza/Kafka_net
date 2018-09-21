using System;
using System.Collections.Generic;
using System.IO;
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
            for (int i = 0; i < 10; i++)
            {
                ProduzirMensagem(i.ToString());    
            }   
        }

        static void Main(string[] args) => new Produce();

        private void ProduzirMensagem(string link)
        {
            var config = new Dictionary<string, object> { { bootstrapServer, uri } };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                EnviarMensagem(producer, link);
            }
        }

        private void EnviarMensagem(Producer<Null, string> producer, string link)
        {
            var dr = producer.ProduceAsync(topic, null, link).Result;
            Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset} {dr.Partition}");
        }

        public void LerArquivo()
        {
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@"links.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    ProduzirMensagem(line);
                }
            }
        }
    }
}
