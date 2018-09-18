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
           
            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            if (question != null)
            {

                int indexOfAttemptedQuestion = userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
                Console.WriteLine("Question response is " + question.userResponse);
                Console.WriteLine("correct option is " + userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption);
                //for(var i=0; i<userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption.Count;i++)
                //{
                //    if (userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption[i].Option == question.userResponse[i])
                //    {

                //            userInfo.MaximumDifficaultyLevelReached++;
                //            userInfo.QuestionBank[indexOfAttemptedQuestion].IsCorrect = true;
                //            userInfo.QuestionBank.Remove(userInfo.QuestionBank[indexOfAttemptedQuestion]);

                //    }
                //}
                if (userInfo.QuestionBank[indexOfAttemptedQuestion].CorrectOption == question.userResponse)
                {
                    userInfo.MaximumDifficaultyLevelReached++;
                    userInfo.QuestionBank[indexOfAttemptedQuestion].IsCorrect = true;
                    userInfo.QuestionBank.Remove(userInfo.QuestionBank[indexOfAttemptedQuestion]);
                }
                else
                {
                    userInfo.MaximumDifficaultyLevelReached--;
                    userInfo.QuestionBank[indexOfAttemptedQuestion].IsCorrect = false;
                }
                userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
                userInfo.QuestionsAttempted.Add(question);
                userInfo.CurrentQuestionIndex += 1;
            }
            if (userInfo.CurrentQuestionIndex < userInfo.QuestionBank.Count)
            {
                var nextQuestion = (from q in userInfo.QuestionBank
                                    where q.DifficultyLevel == userInfo.MaximumDifficaultyLevelReached
                                    select q).First();
                Console.WriteLine("Maximum difficulty level reached " + userInfo.MaximumDifficaultyLevelReached);
                Console.WriteLine("difficulty level of the question sending " + nextQuestion.DifficultyLevel);
                //var save = nextQuestion.CorrectOption;
                //nextQuestion.CorrectOption = null;
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
            userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
            userInfo.QuestionsAttempted.Add(question);           
            _resultService.PostUserInfo(userInfo);          
            userInfo.QuestionBank = null;
            _iquizEngineService.PostUserInfoAsync(userInfo);
            return Clients.Caller.SendAsync("EndQuiz", userInfo);
        }

        public async Task StartQuiz(int userId, string domain)
        {
          
            UserInfo userInfo = new UserInfo();          
            userInfo.UserId = userId;
            userInfo.DomainName = domain;
            userInfo.MaximumDifficaultyLevelReached = 3;           
            _userQuizState.Add(Context.ConnectionId, userInfo);
            userInfo.QuestionBank = await _iquizEngineService.GetQuestionByDomain(domain);
            GetNextQuestion(null);
        }
    }
}

