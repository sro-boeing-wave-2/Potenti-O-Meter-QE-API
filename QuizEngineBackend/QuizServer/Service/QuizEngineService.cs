using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Formatting;
using Potentiometer.Core.QuestionTypes;

namespace QuizServer.Service
{
    public class QuizEngineService : IQuizEngineService
    {
        public static readonly HttpClient _client = new HttpClient();

        public async Task<List<Object>> GetQuestionByDomain(string domain)
        {
            Console.WriteLine("INSIDE GETDOMAIN");
           //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
            var response = await _client.GetAsync("http://localhost:44334/api/questions");
            Console.WriteLine(response.ToString());
            List<Object>  result = await response.Content.ReadAsAsync<List<Object>>();
            //Console.WriteLine("Response from the question bank  IS " + JsonConvert.SerializeObject(result));
            return result;
        }
        public async Task<Object> GetConceptGraph(string domain)
        {
            var response = await _client.GetAsync("");
            return response;
            //not sure how the object look like
        }
        public async Task PostUserInfoAsync(UserInfo userInfo)
        {         
            var response = await _client.PostAsync("http://localhost:5000/api/QuizResult", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
        }
    }
}
