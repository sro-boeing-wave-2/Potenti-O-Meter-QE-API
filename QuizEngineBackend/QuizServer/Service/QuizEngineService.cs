using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Formatting;

namespace QuizServer.Service
{
    public class QuizEngineService : IQuizEngineService
    {
        public static readonly HttpClient _client = new HttpClient();

        public async Task<List<Question>> GetQuestionByDomain(int userId, string domain)
        {
            Console.WriteLine("thiis is inside getQuestionBy Domain");
            var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
            //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/science");
            Console.WriteLine(response);

            //response.options.toString()=>

            var result = await response.Content.ReadAsAsync<List<Question>>();
            //Console.WriteLine("Questions     " + result);
            //foreach (Question q in result)
            //{
            //    Console.WriteLine("Questions     " + JsonConvert.SerializeObject(q.QuestionText));
            //    foreach(var x in q.OptionList)
            //    {
            //        Console.WriteLine(x.Option.ToString());
            //    }
            //}

            return result;

            //var task = await _client.GetAsync("http://localhost:44334/api/questions/domain/maths");
            //var contacts = task.Content.ReadAsAsync(
            //    t => {
            //        return t.Result.Content.ReadAsAsync<List<Question>>();
            //    }).Unwrap().Result;


        }



        public async Task PostUserInfoAsync(UserInfo userInfo)
        {
            //    var userinfo = new UserInfo()
            //{
            //    UserId = 5,
            //    DomainName = "F",
            //    QuizId = "asgklfljkkrkiruti",
            //    QuestionsAttempted = new List<Question>()
            //    {
            //        new Question()
            //        {
            //        QuestionId = "2",
            //        QuestionText = "what is why?",
            //        DifficultyLevel = 1,
            //        userResponse = "It is they",
            //        IsCorrect = true,
            //        OptionList = new List<Options>()
            //            {
            //                new Options()
            //                {
            //                Option = "optA"
            //                },
            //                new Options()
            //                {
            //                Option = "optB"
            //                },
            //               new Options()
            //                {
            //                Option = "optC"
            //                },
            //               new Options()
            //                {
            //                Option = "optD"
            //                }
            //            },
            //        QuestionType = "MCQ"
            //    }
            //    }

            //};

            Console.WriteLine("UserINFO =====>  " + JsonConvert.SerializeObject(userInfo));
            //HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44343/api/QuizResult")
            //{
            //    Content = new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json")
            //};
            //Console.WriteLine("Post Message  -- > " + postMessage);
            var response = await _client.PostAsync("http://172.23.238.183/api/QuizResult", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            Console.WriteLine("RESPONSE -- > " + response);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
            Console.WriteLine("Response String   " + responseString);
            Console.WriteLine("Result   " + result);

        }
    }

}
