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
           
           //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
           
            var response = await _client.GetAsync("http://localhost:44334/api/questions");
            //Console.WriteLine(response.ToString());
            List<Object>  result = await response.Content.ReadAsAsync<List<Object>>();
            //Console.WriteLine("Response from the question bank  IS " + JsonConvert.SerializeObject(result));
            return result;
        }
        public async Task<List<Object>> GetQuestionByIds(List<string> ids)
        {
            List<Object> listofq = new List<Object>();
            List<string> list = new List<string>();
            list.Add("5ba88cc31d3117000149e342");
            list.Add("5ba88cf21d3117000149e345");
            //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
            for(int i=0;i<list.Count;i++)
            {
                var response = await _client.GetAsync("http://localhost:44334/api/questions/id/" + list[i] );
                Object res = await response.Content.ReadAsAsync<Object>();
                
                Console.WriteLine("QUESTION BANK IS " + res);
                listofq.Add(res);
            }
            Console.WriteLine("Question From QUESTION Bank Is " + listofq[0]);
            return listofq;
        }
        public async Task<Object> GetConceptAndConceptToQuestionMap()
        {
            Console.WriteLine("INSIDE GETDOMAIN");
            var response = await _client.GetAsync("http://172.23.238.185:44334/api/questions/questionConceptMap/science");
            Console.WriteLine("INSISE CONSPEC " + response);
            var result = await response.Content.ReadAsAsync<Object>();
            Console.WriteLine("THIS IS THE ANSAER " + result);
            //Console.WriteLine("THIS IS WHAT IM LOOKING FOR " + result);
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
