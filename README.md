# Kafka_net

Produtor e consumidor de mensagem pelo kafka

["https://blogmeninasimples.files.wordpress.com/2012/09/texto-fc3a3.jpg",
"https://i.pinimg.com/originals/63/a6/f7/63a6f7c4fda98e4bce56d35dbb40ed93.jpg",
"https://i.pinimg.com/originals/b1/30/a7/b130a747e79fa6a23697aa3853c94353.jpg",
"http://1.bp.blogspot.com/-Afuwwp8SZPU/TdEuDSbBMGI/AAAAAAAAE-M/4uD_eKTf078/s1600/A+AGULHA+E+A+LINHA.jpg",
"http://4.bp.blogspot.com/_MT0p2_0Lzds/SzGDx0pFSQI/AAAAAAAAIio/iHblIoeRDws/s320/01.JPG",
"http://atividadesprofessores.com.br/wp-content/uploads/2017/01/texto-370x297.jpg",
"https://i.pinimg.com/originals/4e/27/6f/4e276ffa360e6084ff7c7524cb9fe9aa.jpg",
"https://i.pinimg.com/originals/d0/ca/2a/d0ca2aa755d2cfc9c3a7c57112be0bdc.jpg",
"https://fernandabertgalvao.files.wordpress.com/2016/03/wp-1458002133839.jpg"
]

#Rodar o kafka local juntamente com o zookeper
docker run -p 2181:2181 -p 9092:9092 -e ADVERTISED_HOST=127.0.0.1 johnnypark/kafka-zookeeper
