using System;
using System.Collections.Generic;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace kafkaNetProducer
{
    class Produce
    {
        private readonly Uri uri;
        private readonly KafkaOptions options;
        private readonly BrokerRouter router;
        private readonly Producer client;
        private readonly string topic;

        public Produce()
        {
            uri = new Uri("http://localhost:9092");
            options = new KafkaOptions(uri);
            router = new BrokerRouter(options);
            client = new Producer(router);
            topic = "IDGTestTopic";
            ProduzirMensagem();
        }

        static void Main(string[] args) => new Produce();

        private void ProduzirMensagem()
        {

            while (EnviarMensagem())
            {
                Console.WriteLine("Escreva sua mensagem: ");
                string payload = Console.ReadLine();
                Message msg = new Message(payload);
                client.SendMessageAsync(topic, new List<Message> { msg }).Wait();
            }
            Console.WriteLine("Encerrado.");
        }

        public bool EnviarMensagem()
        {
            Console.WriteLine("Deseja enviar mais mensagem? ");
            return Console.ReadLine().Contains("s");
        }

    }
}
