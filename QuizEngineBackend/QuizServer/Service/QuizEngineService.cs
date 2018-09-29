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
<<<<<<< HEAD
           
           //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);
           
            var response = await _client.GetAsync("http://localhost:44334/api/questions");
           
            List<Object>  result = await response.Content.ReadAsAsync<List<Object>>();
           
=======

            //var response = await _client.GetAsync("http://localhost:44334/api/questions/domain/" + domain);

            var response = await _client.GetAsync("http://13.126.26.172/questionbank");
            //Console.WriteLine(response.ToString());
            List<Object> result = await response.Content.ReadAsAsync<List<Object>>();
            //Console.WriteLine("Response from the question bank  IS " + JsonConvert.SerializeObject(result));
>>>>>>> 2a0808ab40a9e19c15afe31aa46d2b6bc30444e1
            return result;
        }
        public async Task<List<Object>> GetQuestionByIds(List<string> ids)
        {
           
            List<Object> listofq = new List<Object>();
<<<<<<< HEAD
           
         
           
            for (int i=0;i<ids.Count;i++)
            {
              
                var response = await _client.GetAsync("http://13.126.26.172/questionbank/id/" + ids[i] );
               
                 Object res = await response.Content.ReadAsAsync<Object>();
=======


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
>>>>>>> 2a0808ab40a9e19c15afe31aa46d2b6bc30444e1
                listofq.Add(res);
            }
            
            return listofq;
        }
        public async Task<Object> GetConceptAndConceptToQuestionMap(string domain)
        {
<<<<<<< HEAD
            
            var response = await _client.GetAsync("http://13.126.26.172/questionbank/questionConceptMap/" + domain);
           
=======
            Console.WriteLine("INSIDE GETDOMAIN");
            var response = await _client.GetAsync("http://13.126.26.172/questionbank/questionConceptMap/" + domain);
            Console.WriteLine("INSISE CONSPEC " + response);
>>>>>>> 2a0808ab40a9e19c15afe31aa46d2b6bc30444e1
            var result = await response.Content.ReadAsAsync<Object>();
           
            return result;
        }
       
        public async Task PostUserInfoAsync(UserInfo userInfo)
        {
<<<<<<< HEAD
           Console.WriteLine("ENDING THE QUIZ " + JsonConvert.SerializeObject(userInfo));
=======
            Console.WriteLine("ENDING THE QUIZ " + JsonConvert.SerializeObject(userInfo));
>>>>>>> 2a0808ab40a9e19c15afe31aa46d2b6bc30444e1
            var response = await _client.PostAsync("http://13.126.26.172/result", new StringContent(JsonConvert.SerializeObject(userInfo), UnicodeEncoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<UserInfo>();
        }
    }
}
