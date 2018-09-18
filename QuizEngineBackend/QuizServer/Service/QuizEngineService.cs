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

        public async Task<List<Question>> GetQuestionByDomain(string domain)
        {         
            var response = await _client.GetAsync("http://172.23.238.185:44334/api/questions/domain/" + domain);       
            var result = await response.Content.ReadAsAsync<List<Question>>();            
            return result;
        }

        public async Task PostUserInfoAsync(UserInfo userInfo)
        {         
            var response = await _client.PostAsync("http://localhost:5000/api/QuizResult", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
        }
    }
}
