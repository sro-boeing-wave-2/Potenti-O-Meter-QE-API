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

                userInfo.QuestionsAttempted.Add(question);
               // JObject ParsedQuestion = JObject.Parse(JsonConvert.SerializeObject(question));
               // string questionType = ParsedQuestion.GetValue("questionType").ToString();               
               // System.Reflection.Assembly b = System.Reflection.Assembly.Load("Potentiometer.Core");
               // Type QuestionType = b.GetType("Potentiometer.Core.QuestionTypes." + questionType);               
               // object instanceObjectOfQuestion = Activator.CreateInstance(QuestionType);               
               // JsonConvert.PopulateObject(JsonConvert.SerializeObject(question), instanceObjectOfQuestion);               
               // bool result = (bool)QuestionType.InvokeMember("Evaluate", BindingFlags.InvokeMethod, null, instanceObjectOfQuestion, new object[0]);
              
               //if(result == true)
               //{
               //     _graphService.UpdateUserConceptRelation(ParsedQuestion.GetValue("questionId").ToString(), userInfo.UserId);
               // }
               //else
               // {
               //     _graphService.UpdateUserConceptRelationForWrongQuestion(ParsedQuestion.GetValue("questionId").ToString(), userInfo.UserId);
               // }         
            }
            if (userInfo.CurrentQuestionIndex < userInfo.QuestionsFromQuestionBank.Count)
            {
                try
                {
                   
                    var x = JsonConvert.SerializeObject(userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex]);
                    JArray z = JArray.Parse(x);
                    JToken q = z[0];              
                    var qtype = q["questionType"].Value<string>();
                    Potentiometer.Core.QuestionTypes.MCQ q1 = new Potentiometer.Core.QuestionTypes.MCQ();
                    System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                    Type type1 = a.GetType("Potentiometer.Core.QuestionTypes."+qtype);
                    object instanceObject = Activator.CreateInstance(type1);
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(q), instanceObject);                 
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


        public  Task EndQuiz(Object question)
        {
            UserInfo userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            //int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);           
            userInfo.QuestionsAttempted.Add(question);
            userInfo.QuizId = userInfo.CurrentQuestionIndex + "ktya9b37jh47hkqw700017a8y" + userInfo.UserId;
            userInfo.QuestionsFromQuestionBank = null;
            _iquizEngineService.PostUserInfoAsync(userInfo);
            //await _resultService.PostUserInfo(userInfo);
            //UserInfo ui = await _resultService.GetByID(userInfo.UserId);
            
            return Clients.Caller.SendAsync("EndQuiz", userInfo);
        }

        public async Task StartQuiz(int userId, string domain)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UserId = userId;
            userInfo.DomainName = domain;
            _userQuizState.Add(Context.ConnectionId, userInfo);
            var ConceptAndConceptToQuestionMap = await _iquizEngineService.GetConceptAndConceptToQuestionMap(domain);
            var stringForm = JsonConvert.SerializeObject(ConceptAndConceptToQuestionMap);
            var ConceptMapandConcepttoQuestionMap = JArray.Parse(stringForm);
            var version = ConceptMapandConcepttoQuestionMap[0]["version"];
            var domainForConceptGraph = ConceptMapandConcepttoQuestionMap[0]["domain"];
            List<Triplet> questionConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["questionconceptTriplet"].ToObject<List<Triplet>>();
            List<ConceptMap> ConceptToConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["concepttriplet"].ToObject<List<ConceptMap>>();
            bool IsDomainExist = _graphService.IsDomainExist((string)domainForConceptGraph);
            bool IsUser = _graphService.IsUserExist(userInfo.UserId);
            if(IsUser != true)
            {
                _graphService.CreateUser(userInfo.UserId);
            }
            if (IsDomainExist != true)
            {
                var resul = _graphService.CreateConceptToQuestionMapping(questionConceptTriplet, (string)version, (string)domainForConceptGraph);
                var resultOfConceptToConceptMapping = _graphService.CreateConceptToConceptMapping(ConceptToConceptTriplet);
              
            }
            //get the questions from the graph 
            //_graphService.GetQuestionsFromGraph(userInfo.UserId, userInfo.DomainName);
            JToken QuestionIDs = ConceptMapandConcepttoQuestionMap[0]["questionIds"];
            List<string> Q = new List<string>();
            foreach(string x in QuestionIDs)
            {
                Console.WriteLine(x.GetType());
                Q.Add(x);
            }
            var QuestionID = JsonConvert.SerializeObject(QuestionIDs);
            userInfo.QuestionsFromQuestionBank = await _iquizEngineService.GetQuestionByIds(Q);
            
            //var result = _graphService.GetGraph((string)domainForConceptGraph);
            //_graphService.GetGraph((string)domainForConceptGraph);
           
             
            GetNextQuestion(null);
        }
    }
}

