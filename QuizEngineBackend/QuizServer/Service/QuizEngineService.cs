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

        public async Task<List<Question>> GetQuestionByDomain()
        {
            Console.WriteLine("thiis is inside getQuestionBy Domain");
            var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/maths");
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

        public async Task PostUserInfoAsync(UserInfo userinfo)
        {
            HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8083/api/userinfo")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userinfo), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(postMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }

}
