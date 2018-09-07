using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using QuizServer.Models;

namespace QuizServer
{
    public class QuestionHub : Hub
    {   
        private static Dictionary<string, UserInfo> _userQuizState = new Dictionary<string, UserInfo>();
        

        public Task GetNextQuestion(Question question)
        {
            Console.WriteLine("connection ID " + Context.ConnectionId);
            // Update the user's response in the Question present in the Question Bank
            var userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            Console.WriteLine("Inside get next question");
            // If the question exists then update the response of the question 
            if (question != null)
            {
                Console.WriteLine("\nQuestion" + question);
                Console.WriteLine("\nUsercount " + _userQuizState.Count);
                Console.WriteLine(question.QuestionText);  
                Console.WriteLine("\nUser Info " + userInfo);             
                int indexOfAttemptedQuestion =  userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
                userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
                //count++;
                userInfo.CurrentQuestionIndex += 1;
            }

            if (userInfo.CurrentQuestionIndex < userInfo.QuestionBank.Count)
            {
                Console.WriteLine("count " + userInfo.CurrentQuestionIndex);
                Console.WriteLine("user info checking " + userInfo.UserId  );
                var nextQuestion = userInfo.QuestionBank[userInfo.CurrentQuestionIndex];
                return Clients.Caller.SendAsync("NextQuestion", nextQuestion);
            }
            else
            {
                Console.WriteLine("count " + userInfo.CurrentQuestionIndex);
                Console.WriteLine("user info checking " + userInfo.UserId);
                EndQuiz(question);
                return Clients.Caller.SendAsync("EndQuiz");
            }
        }

        public Task EndQuiz(Question question)
        {

            UserInfo userInfo = _userQuizState.GetValueOrDefault(Context.ConnectionId);
            int indexOfAttemptedQuestion =  userInfo.QuestionBank.FindIndex(q => q.QuestionId == question.QuestionId);
            Console.WriteLine("Inside ENDQUIZ indexOfAttemptedQuestion " + indexOfAttemptedQuestion);
            userInfo.QuestionBank[indexOfAttemptedQuestion].userResponse = question.userResponse;
            Console.WriteLine("userid in the serve " + userInfo.UserId);
            return Clients.Caller.SendAsync("EndQuiz",userInfo);
           // return _userQuizState.GetValueOrDefault(Context.ConnectionId);

            // Also store a copy of the UserQuizState in the Database
            // Do a post request to analytics microservice
        }

        public Task StartQuiz(int userId)
        {
            // Needs to generate a quizid
            int userID = userId;
            //Console.WriteLine("this is startQuiz " + userId);
            UserInfo userInfo = new UserInfo();
            userInfo.UserId = userID;
            // Should have the logic of getting the questions sometime later
            _userQuizState.Add(Context.ConnectionId, userInfo);
            Console.WriteLine("Inside start quiz " + _userQuizState.Count);
            return GetNextQuestion(null);
        }
    }
}
