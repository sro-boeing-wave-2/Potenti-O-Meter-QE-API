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

            var response = await _client.GetAsync("http://13.126.26.172/questionbank");
            //Console.WriteLine(response.ToString());
            List<Object> result = await response.Content.ReadAsAsync<List<Object>>();
            //Console.WriteLine("Response from the question bank  IS " + JsonConvert.SerializeObject(result));
            return result;
        }
        public async Task<List<Object>> GetQuestionByIds(List<string> ids)
        {
            Console.WriteLine("HERE");
            List<Object> listofq = new List<Object>();


            //var response = await _client.GetAsync("http://172.238.232.188:44334/api/questions/domain/" + domain);
            for (int i = 0; i < ids.Count; i++)
            {
                //Console.WriteLine("http://172.23.238.185:44334/api/questions/id/" + ids[i]);
                var response = await _client.GetAsync("http://13.126.26.172/questionbank/id/" + ids[i]);
                Object res = await response.Content.ReadAsAsync<Object>();
                //Console.WriteLine("QUESTION BANK IS hhhh " + res);
                //var x = JsonConvert.SerializeObject(res);
                //Console.WriteLine("QUESTION BANK IS hhhh " + x);
                //JArray z = JArray.Parse(x);
                //JToken q = z[0];
                //Console.WriteLine("QUESTION BANK IS hhhh " + q);
                listofq.Add(res);
            }
            //Console.WriteLine("Question From QUESTION Bank Is " + listofq[0]);
            return listofq;
        }
        public async Task<Object> GetConceptAndConceptToQuestionMap(string domain)
        {
            Console.WriteLine("INSIDE GETDOMAIN");
            var response = await _client.GetAsync("http://13.126.26.172/questionbank/questionConceptMap/" + domain);
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
            Console.WriteLine("ENDING THE QUIZ " + JsonConvert.SerializeObject(userInfo));
            var response = await _client.PostAsync("http://13.126.26.172/result", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
        }
    }
}
