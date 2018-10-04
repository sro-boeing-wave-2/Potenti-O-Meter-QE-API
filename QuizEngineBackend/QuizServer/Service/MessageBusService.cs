using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizServer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizServer.Service
{
    public class MessageBusService : IMessageBusService
    {
        private IGraphService _graphService;

        public MessageBusService(IGraphService graphService)
        {
            _graphService = graphService;



            string consulIP = Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
            var factory = new ConnectionFactory() { HostName = consulIP, UserName = "preety", Password = "preety", Port = 5672 };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            Console.WriteLine("LISTENINIG ");

            channel.QueueDeclare(queue: "ConceptMap",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine("Listening on Queue");
            consumer.Received += (model, ea) =>
            {
               
                var message = ea.Body;
                var body = Encoding.UTF8.GetString(message);              
                var ConceptMapandConcepttoQuestionMap = JObject.Parse(body);
                var version = ConceptMapandConcepttoQuestionMap["Version"].Value<string>();
                var domainForConceptGraph = ConceptMapandConcepttoQuestionMap["Domain"];
                //bool IsDomainExist = _graphService.IsDomainExist((string)domainForConceptGraph);
                List<Triplet> questionConceptTriplet = ConceptMapandConcepttoQuestionMap["questionconceptTriplet"].ToObject<List<Triplet>>();
                Console.WriteLine("THIS IS FROM MESSAGE QUEUE " + body);
                List<ConceptMap> ConceptToConceptTriplet = ConceptMapandConcepttoQuestionMap["concepttriplet"].ToObject<List<ConceptMap>>();
                List<ContentConceptTriplet> ConceptToContenttTriplet = ConceptMapandConcepttoQuestionMap["contentConceptTriplet"].ToObject<List<ContentConceptTriplet>>();
                var result = _graphService.CreateConceptToQuestionMapping(questionConceptTriplet, (string)version, (string)domainForConceptGraph);
                var resultOfConceptToConceptMapping = _graphService.CreateConceptToConceptMapping(ConceptToConceptTriplet, (string)domainForConceptGraph);
                var resultOfContentRecommenderMapping = _graphService.CreateContentRecommendationMapping(ConceptToContenttTriplet, (string)domainForConceptGraph);
            };
            channel.BasicConsume(queue: "ConceptMap",
                                 autoAck: true,
                                 consumer: consumer);


        }
    }
}


