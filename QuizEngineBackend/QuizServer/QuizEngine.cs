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
using System.Reflection;

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

        public Task GetNextQuestion(Object question)
        {
            
            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            if (question != null)
            {
                Console.WriteLine("INSIDE WHAT ");
               

                var x = JsonConvert.SerializeObject(question);
               
                userInfo.QuestionsAttempted.Add(x);
                //JObject z = JObject.Parse(x);
                ////JToken q = z[0];
                //Console.WriteLine("PRINTING QUESTION TYPE");
                ////Console.WriteLine(z["questionType");
                //Console.WriteLine(z.GetValue("questionType"));
                //var qtype = z["questionType"].Value<string>();
                //Potentiometer.Core.QuestionTypes.MCQ q1 = new Potentiometer.Core.QuestionTypes.MCQ();
                //System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                //Type type1 = a.GetType("Potentiometer.Core.QuestionTypes." + qtype);
                //object instanceObject = Activator.CreateInstance(type1);
                //JsonConvert.PopulateObject(JsonConvert.SerializeObject(z), instanceObject);
                //Console.WriteLine("POPULATED OBJECT Second TIME" + instanceObject);
                //Console.WriteLine("THIS IS FROM FRONT END " + question);
                //System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                //Type type1 = a.GetType("Potentiometer.Core.QuestionTypes." + qtype);
                //object instanceObject = Activator.CreateInstance(type1);
                //JsonConvert.PopulateObject(JsonConvert.SerializeObject(question), instanceObject);
                //bool result = (bool)type1.InvokeMember("Evaluate", BindingFlags.InvokeMethod, null, instanceObject, new object[0]);
                //userInfo.QuestionsAttempted.Add(question);

                //int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
                //Console.WriteLine("Question response is " + question.userResponse);
                //Console.WriteLine("correct option is " + userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption);
                // (userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex].CorrectOption == question.userResponse)
                //{
                //   userInfo.MaximumDifficaultyLevelReached++;
                //    userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex].IsCorrect = true;
                //   userInfo.QuestionBank.Remove(userInfo.QuestionBank[indexOfAttemptedQuestion]);
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
                    //Type type = userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex].GetType();

                    var x = JsonConvert.SerializeObject(userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex]);
                    JArray z = JArray.Parse(x);
                    JToken q = z[0];
                    Console.WriteLine("PRINTING QUESTION TYPE");
                    Console.WriteLine(q["questionType"]);
                    //var q = (JObject)userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex];

                    var qtype = q["questionType"].Value<string>();
                    Potentiometer.Core.QuestionTypes.MCQ q1 = new Potentiometer.Core.QuestionTypes.MCQ();
                    System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                    Type type1 = a.GetType("Potentiometer.Core.QuestionTypes."+qtype);
                    object instanceObject = Activator.CreateInstance(type1);
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(q), instanceObject);
                    Console.WriteLine("POPULATED OBJECT " + instanceObject);
                    userInfo.CurrentQuestionIndex= userInfo.CurrentQuestionIndex + 1;
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
                //EndQuiz(question);
                return Clients.Caller.SendAsync("EndQuiz");
            }
        }


        public Task EndQuiz(Question question)
        {
            UserInfo userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            //int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
            // userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
            // userInfo.QuestionsAttempted.Add(question);
            var x = JsonConvert.SerializeObject(question);

            userInfo.QuestionsAttempted.Add(x);
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
            //userInfo.MaximumDifficaultyLevelReached = 3;
            _userQuizState.Add(Context.ConnectionId, userInfo);
            var g = await _iquizEngineService.GetConceptAndConceptToQuestionMap();
           
            var stringForm = JsonConvert.SerializeObject(g);
            var ConceptMapandConcepttoQuestionMap = JArray.Parse(stringForm);
            var version = ConceptMapandConcepttoQuestionMap[0]["version"];
            var domainForConceptGraph = ConceptMapandConcepttoQuestionMap[0]["domain"];
            List<Triplet> questionConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["questionconceptTriplet"].ToObject<List<Triplet>>();
            List<ConceptMap> ConceptToConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["concepttriplet"].ToObject<List<ConceptMap>>();
            bool IsDomainExist = _graphService.IsDomainExist((string)domainForConceptGraph);
            
            
                var result = _graphService.CreateConceptToQuestionMapping(questionConceptTriplet, (string)version, (string)domainForConceptGraph);
                var resultOfConceptToConceptMapping = _graphService.CreateConceptToConceptMapping(ConceptToConceptTriplet);
            
            JToken QuestionIDs = ConceptMapandConcepttoQuestionMap[0]["questionIds"];
            List<string> Q = new List<string>();
            foreach(string x in QuestionIDs)
            {
                Console.WriteLine(x.GetType());
                Q.Add(x);
            }
            var QuestionID = JsonConvert.SerializeObject(QuestionIDs);
            Console.WriteLine("PRINTINGGGGGGGGGGGGGGGGG" + Q[0]);
            Console.WriteLine(Q[0]);
           // var QuestionIDs = ConceptMapandConcepttoQuestionMap[0]["questionIds"];
            //string[] QuestionID = { "5ba8c4181df1b6000184a58c", "5ba88cf21d3117000149e345" };
           userInfo.QuestionsFromQuestionBank = await _iquizEngineService.GetQuestionByIds(Q);
            //var obj = _graphService.GetGraph();
            Console.WriteLine("Question Bank " + userInfo.QuestionsFromQuestionBank[0]);
            GetNextQuestion(null);
        }
    }
}

