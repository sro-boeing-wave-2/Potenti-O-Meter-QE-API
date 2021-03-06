﻿using System;
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
        //private static Dictionary<int , string> _userConnectionState = new Dictionary<int , string>();
        //private static Dictionary<int, UserInfo> _userQuizState = new Dictionary<int,UserInfo>();
        private IQuizEngineService _iquizEngineService;

        private IResultService _resultService;
        private IGraphService _graphService;

        public static readonly HttpClient _client = new HttpClient();
        List<Object> questionsToBeAdded = new List<Object>();


        public QuestionHub(IQuizEngineService iquizEngineService, IResultService resultService, IGraphService graphService)
        {
            _iquizEngineService = iquizEngineService;
            _resultService = resultService;
            _graphService = graphService;
        }

        public Task GetNextQuestion(Object question)
        {
            //var userInfo = _userQuizState.GetValueOrDefault(userId);
            //var userConnection = _userConnectionState(userId);
            //var connectionAlive = heartBeat.GetConnections().FirstOrDefault(c=>c.ConnectionId == connection.ConnectionId);
           
            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            if (question != null)
            {
                
                userInfo.QuestionsAttempted.Add(question);
               
               
            }
            if (userInfo.CurrentQuestionIndex < userInfo.QuestionsFromQuestionBank.Count)
            {
                try
                {
                    
                    var x = JsonConvert.SerializeObject(userInfo.QuestionsFromQuestionBank[userInfo.CurrentQuestionIndex]);
                   
                    var q = JObject.Parse(x);
                   
                    //JArray z = JArray.Parse(x);
                    //JToken q = z[0];              
                    var qtype = q["questionType"].Value<string>();
                    Potentiometer.Core.QuestionTypes.MCQ q1 = new Potentiometer.Core.QuestionTypes.MCQ();
                    System.Reflection.Assembly a = System.Reflection.Assembly.Load("Potentiometer.Core");
                    Type type1 = a.GetType("Potentiometer.Core.QuestionTypes."+qtype);
                    object instanceObject = Activator.CreateInstance(type1);
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(q), instanceObject);                 
                    userInfo.CurrentQuestionIndex= userInfo.CurrentQuestionIndex + 1;
                    Console.WriteLine("This is the question I'm sending " + JsonConvert.SerializeObject( instanceObject));
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


        public async Task EndQuiz(Object question)
        {
            UserInfo userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
                     
            userInfo.QuestionsAttempted.Add(question);
            
            questionsToBeAdded = userInfo.QuestionsAttempted;
           
            userInfo.QuestionsFromQuestionBank = null;
            _graphService.UpdateUserConceptRelation(userInfo, userInfo.UserId);          
            userInfo.QuestionsAttempted = null;
            await _resultService.PostUserInfo(userInfo);
            
           
            UserInfo ui = await _resultService.GetByID(userInfo.UserId);
            ui.QuestionsAttempted = questionsToBeAdded;
            Console.WriteLine("THIS IS THE RESULT " + JsonConvert.SerializeObject(ui));
            await _iquizEngineService.PostUserInfoAsync(ui);
          
           await _resultService.DeleteByIdAsync(userInfo.UserId);

            var Contents = _graphService.GetContentRecommendations(userInfo.UserId,userInfo.DomainName);
            Console.WriteLine("THIS IS THE CONTENT RECOMMENDER " + JsonConvert.SerializeObject(Contents));
            Clients.Caller.SendAsync("EndQuiz", ui);
        }

        public async Task StartQuiz(int userId, string domain)
        {
            UserInfo userInfo = new UserInfo();
            
            userInfo.UserId = userId;
            userInfo.DomainName = domain;
            _userQuizState.Add(Context.ConnectionId, userInfo);
            bool IsDomainExist = _graphService.IsDomainExist(domain);
            Console.WriteLine(IsDomainExist);
            //if (IsDomainExist != true)
            //{
            //    var ConceptAndConceptToQuestionMap = await _iquizEngineService.GetConceptAndConceptToQuestionMap(domain);
            //    var stringForm = JsonConvert.SerializeObject(ConceptAndConceptToQuestionMap);
            //    var ConceptMapandConcepttoQuestionMap = JArray.Parse(stringForm);
            //    var version = ConceptMapandConcepttoQuestionMap[0]["version"];
            //    var domainForConceptGraph = ConceptMapandConcepttoQuestionMap[0]["domain"];
            //    List<Triplet> questionConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["questionconceptTriplet"].ToObject<List<Triplet>>();
            //    List<ConceptMap> ConceptToConceptTriplet = ConceptMapandConcepttoQuestionMap[0]["concepttriplet"].ToObject<List<ConceptMap>>();
            //    var resul = _graphService.CreateConceptToQuestionMapping(questionConceptTriplet, (string)version, (string)domainForConceptGraph);
            //    var resultOfConceptToConceptMapping = _graphService.CreateConceptToConceptMapping(ConceptToConceptTriplet, (string)domainForConceptGraph);

            //}
            bool IsUser = _graphService.IsUserExist(userInfo.UserId);
            if (IsUser != true)
            {
                _graphService.CreateUser(userInfo.UserId);
            }
            if(IsDomainExist)
            {
                List<string> QuestionsId = _graphService.GetQuestionsFromGraph(userInfo.UserId, userInfo.DomainName);
                Console.WriteLine("THIS IS THE " + JsonConvert.SerializeObject(QuestionsId));
                userInfo.QuestionsFromQuestionBank = await _iquizEngineService.GetQuestionByIds(QuestionsId);
            }
            
            Console.WriteLine("THIS IS THE " + JsonConvert.SerializeObject(userInfo.QuestionsFromQuestionBank));
            GetNextQuestion(null);

        }
    }
}

    