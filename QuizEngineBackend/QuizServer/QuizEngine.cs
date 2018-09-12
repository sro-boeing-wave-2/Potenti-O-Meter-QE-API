using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QuizServer.Models;
using QuizServer.Service;

namespace QuizServer
{
    public class QuestionHub : Hub
    {
        private static Dictionary<string, UserInfo> _userQuizState = new Dictionary<string, UserInfo>();

        private IQuizEngineService _iquizEngineService;

        private IResultService _resultService;

        public static readonly HttpClient _client = new HttpClient();

        public QuestionHub(IQuizEngineService iquizEngineService, IResultService resultService)
        {

            _iquizEngineService = iquizEngineService;
            _resultService = resultService;
        }

        public Task GetNextQuestion(Question question)
        {
            Console.WriteLine("connection ID " + Context.ConnectionId);
            // Update the user's response in the Question present in the Question Bank
            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            Console.WriteLine("Inside get next question");
            // If the question exists then update the response of the question 
           
            if (question != null)
            {
                
                int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
                userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
               // userInfo.QuestionBank[indexOfAttemptedQuestion].
                userInfo.CurrentQuestionIndex += 1;
            }

            if (userInfo.CurrentQuestionIndex < userInfo.QuestionBank.Count)
            {
               var nextQuestion = userInfo.QuestionBank[userInfo.CurrentQuestionIndex];
               //Question nextQuestion = (from q in userInfo.QuestionBank
               //                             where q.DifficultyLevel == userInfo.MaximumDifficaultyLevelReached
               //                             select q).First();
              //String nextQuestion.CorrectOption
                return Clients.Caller.SendAsync("NextQuestion", nextQuestion);
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
            int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
            Console.WriteLine("Inside ENDQUIZ indexOfAttemptedQuestion " + indexOfAttemptedQuestion);
            userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
            Console.WriteLine("userid in the serve " + userInfo.UserId);
            Console.WriteLine("user info " + (userInfo));
            _resultService.PostUserInfo(userInfo);
            Console.WriteLine("========================== between two ================================");
            _iquizEngineService.PostUserInfoAsync(userInfo);
            
           return Clients.Caller.SendAsync("EndQuiz", userInfo);
            // return _userQuizState.GetValueOrDefault(Context.ConnectionId);

            // Also store a copy of the UserQuizState in the Database
            // Do a post request to analytics microservice
        }

        public async Task StartQuiz(int userId, string domain)
        {
            Console.WriteLine("This is inside start" + domain);
            // Needs to generate a quizid
            //int userID = userId;
            //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/science");
            //Console.WriteLine(response);
            //var result = await response.Content.ReadAsAsync<List<Question>>();
            //Console.WriteLine("Questions     " + result);
            //foreach(Question q in result)
            //{
            //    Console.WriteLine("Each Question ------>  " + q);
            //    foreach(var x in q.OptionList)
            //    {
            //        Console.WriteLine("Each Question ------>  " + x.Option);
            //    }
            //}
            UserInfo userInfo = new UserInfo();
            //UserInfo userInfo = new UserInfo(_iquizEngineService, result);
            userInfo.UserId = userId;
            userInfo.DomainName = domain;
            //userInfo.MaximumDifficaultyLevelReached = 3;
            // Should have the logic of getting the questions sometime later
            _userQuizState.Add(Context.ConnectionId, userInfo);
            userInfo.QuestionBank = await _iquizEngineService.GetQuestionByDomain(userId, domain);
            userInfo.QuestionsAttempted = await _iquizEngineService.GetQuestionByDomain(userId, domain);
            Console.WriteLine("END OF START");
            GetNextQuestion(null);
        }
    }
}

