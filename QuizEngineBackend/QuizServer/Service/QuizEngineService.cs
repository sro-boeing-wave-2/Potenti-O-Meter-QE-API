using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
namespace QuizServer.Service
{
    public class QuizEngineService : IQuizEngineService
    {
        public static readonly HttpClient _client = new HttpClient();

        public async Task GetQuestionByDomain()
        {
            Console.WriteLine("thiis is inside getQuestionBy Domain");
            var response = await _client.GetAsync("http://172.23.238.185:44334/api/questions/domain/{science}");
            Console.WriteLine(response);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            
         
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
