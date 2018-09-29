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
using Newtonsoft.Json.Linq;

namespace QuizServer.Service
{
    public class QuizEngineService : IQuizEngineService
    {
        public static readonly HttpClient _client = new HttpClient();

        public async Task<List<Object>> GetQuestionByDomain(string domain)
        {
           
           //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
           
            var response = await _client.GetAsync("http://localhost:44334/api/questions");
           
            List<Object>  result = await response.Content.ReadAsAsync<List<Object>>();
           
            return result;
        }
        public async Task<List<Object>> GetQuestionByIds(List<string> ids)
        {
           
            List<Object> listofq = new List<Object>();
           
         
           
            for (int i=0;i<ids.Count;i++)
            {
              
                var response = await _client.GetAsync("http://13.126.26.172/questionbank/id/" + ids[i] );
               
                 Object res = await response.Content.ReadAsAsync<Object>();
                listofq.Add(res);
            }
            
            return listofq;
        }
        public async Task<Object> GetConceptAndConceptToQuestionMap(string domain)
        {
            
            var response = await _client.GetAsync("http://13.126.26.172/questionbank/questionConceptMap/" + domain);
           
            var result = await response.Content.ReadAsAsync<Object>();
           
            return result;
        }
       
        public async Task PostUserInfoAsync(UserInfo userInfo)
        {
           Console.WriteLine("ENDING THE QUIZ " + JsonConvert.SerializeObject(userInfo));
            var response = await _client.PostAsync("http://13.126.26.172/result", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
        }
    }
}
