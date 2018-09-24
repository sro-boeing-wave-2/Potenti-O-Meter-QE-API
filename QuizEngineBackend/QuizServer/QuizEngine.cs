using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QuizServer.Models;
using QuizServer.Service;
using Potentiometer.Core.QuestionTypes;
using Newtonsoft.Json.Linq;

namespace QuizServer
{
    public class QuestionHub : Hub
    {
        private static Dictionary<string, UserInfo> _userQuizState = new Dictionary<string, UserInfo>();

        private IQuizEngineService _iquizEngineService;

        private IResultService _resultService;
        private IGraphService _graphService;

        public static readonly HttpClient _client = new HttpClient(); 

        public QuestionHub(IQuizEngineService iquizEngineService, IResultService resultService, IGraphService graphService)
        {

            _iquizEngineService = iquizEngineService;
            _resultService = resultService;
            _graphService = graphService;

        }

        public Task GetNextQuestion(Question question)
        {

            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            if (question != null)
            {
                //should get the question from the question on the basis of ID's in the concept graph
                //int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
                //Console.WriteLine("Question response is " + question.userResponse);
                //Console.WriteLine("correct option is " + userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption);
                //if (userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption == question.userResponse)
                //{
                //    userInfo.MaximumDifficaultyLevelReached++;
                //    userInfo.QuestionBank[indexOfAttemptedQuestion].IsCorrect = true;
                //    userInfo.QuestionBank.Remove(userInfo.QuestionBank[indexOfAttemptedQuestion]);
                //}
                //else
                //{
                //    userInfo.MaximumDifficaultyLevelReached--;
                //    userInfo.QuestionBank[indexOfAttemptedQuestion].IsCorrect = false;
                //}
                //userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
                //userInfo.QuestionsAttempted.Add(question);
                //userInfo.CurrentQuestionIndex += 1;
            }
            if (userInfo.CurrentQuestionIndex < userInfo.QuestionsFromQuestionBank.Count)
            {
                try
                {
                    //var qu = userInfo.ConceptGraph.triplet[0].target.QuestionId;
                    Console.WriteLine("========================" + userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex]);
                    Type type = userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex].GetType();

                    var q = (JObject)userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex];

                    var qtype = q["questionType"].Value<string>();
                    Potentiometer.Core.QuestionTypes.MCQ q1 = new Potentiometer.Core.QuestionTypes.MCQ();
                    System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                    Type type1 = a.GetType("Potentiometer.Core.QuestionTypes."+qtype);
                    Console.WriteLine(System.Reflection.Assembly.GetAssembly(typeof(Potentiometer.Core.QuestionTypes.MCQ)).FullName.ToString());
                    Console.WriteLine("TYPE11111 " + type1);

                    Console.WriteLine("TYPE OF THE QUESTION " + "Potentiometer.Core." + qtype);
                    object instanceObject = Activator.CreateInstance(type1);
                    Console.WriteLine("INSTANCE OF THE OBJECT " + instanceObject);
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex]), instanceObject);
                    Console.WriteLine("POPULATED OBJECT " + instanceObject);
                    //userInfo.MaximumDifficaultyLevelReached = nextQuestion.DifficultyLevel;
                    return Clients.Caller.SendAsync("NextQuestion", instanceObject);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                return Clients.Caller.SendAsync("NextQuestion", "");
            }
            else
            {
                EndQuiz(question);
                return Clients.Caller.SendAsync("EndQuiz");
            }
        }


        public Task EndQuiz(Question question)
        {
            UserInfo userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            //int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
           // userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
           // userInfo.QuestionsAttempted.Add(question);
            _resultService.PostUserInfo(userInfo);
            //userInfo.QuestionBank = null;
            _iquizEngineService.PostUserInfoAsync(userInfo);
            Console.WriteLine("RESULT IS " + JsonConvert.SerializeObject(userInfo));
            return Clients.Caller.SendAsync("EndQuiz", userInfo);
        }

        public async Task StartQuiz(int userId, string domain)
        {

            UserInfo userInfo = new UserInfo();
            userInfo.UserId = userId;
            userInfo.DomainName = domain;
            userInfo.MaximumDifficaultyLevelReached = 3;
            _userQuizState.Add(Context.ConnectionId, userInfo);
            bool IsDomainExist = _graphService.IsDomainExist(domain);
           
            userInfo.QuestionsFromQuestionBank = await _iquizEngineService.GetQuestionByDomain(domain);
            Console.WriteLine("afetr questions");
            //var obj = //call to the quesstion bank;
            //get the version and domain seperately as a string
            //send them to with the triplet to form a graph;
            //List<Triplet> questionConceptTriplet = obj['questionConceptTriplet']Value<List<Triplet>();;
            List<Triplet> triplet = new List<Triplet>()
            {
                new Triplet()
                {
                   source =  new Concept()
                    {
                        ConceptName="chemistry",
                         domain = "science"
                    },
                    target = new QuestionIdNode()
                    {
                        QuestionId =  "5db1b4f3d5c1a8cda768a"
                    },

                    relationship = new Relationship()
                    {
                        name = "forget"
                    }
                },
               
                 new Triplet()
                {
                   source =  new Concept()
                    {
                        ConceptName="biology",
                         domain = "science"
                    },
                    target = new QuestionIdNode()
                    {
                        QuestionId =  "78db1b4f3d5c1a8cda768a"
                    },

                    relationship = new Relationship()
                    {
                        name = "remember"
                    }
                },
                  new Triplet()
                {
                   source =  new Concept()
                    {
                        ConceptName="physics",
                         domain = "science"
                    },
                    target = new QuestionIdNode()
                    {
                        QuestionId =  "895db1b4f3d5c1a8cda768a"
                    },

                    relationship = new Relationship()
                    {
                        name = "understanding"
                    }
                }

            };
            List<ConceptMap> conceptMap = new List<ConceptMap>()
            {
                new ConceptMap()
                {
                    Source = new Concept()
                    {
                        ConceptName="physics",
                         domain = "science"
                    },
                    Target = new Concept()
                    {
                        ConceptName = "biology",
                        domain = "science"
                    },
                    Predicate = new Relationship()
                    {
                        name = "Subconcept_of"
                    }


                }
            };
            if (!IsDomainExist)
            {
                var result = _graphService.CreateConceptToQuestionMapping(triplet);
                var resultOfConceptToConceptMapping = _graphService.CreateConceptToConceptMapping(conceptMap);
            }
            
            Console.WriteLine("THIS IS WHAT I'M LOOKING FOR " + IsDomainExist);

            //QuestionIdNode q = new QuestionIdNode()
            //{
            //    QuestionId = "5db1b4f3d5c1a8cda768a"
            //};
            //var result = _graphService.CreateQuestionIdNode(q);
            //Console.WriteLine("GRAPH DATA " + JsonConvert.SerializeObject(result));

            //List<ConceptMap> conceptMap = obj['concepttriplet'].Value<List<ConceptMap>();

            //Console.WriteLine("QUESTIONS -- > " + userInfo.QuestionsFromQuestionBank);
            //Console.WriteLine("questions inside ENGINE " +JsonConvert.SerializeObject( userInfo.QuestionsFromQuestionBank));
            //userInfo.ConceptGraph = await _iquizEngineService.GetConceptGraph(domain);
            // userInfo.QuestionBank = await _iquizEngineService.GetQuestionByDomain(domain);
            //getting the concept graph from the concept graph MS


            GetNextQuestion(null);
        }
    }
}

