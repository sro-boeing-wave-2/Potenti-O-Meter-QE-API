using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class ContentRecommender
    {
        public string url { get; set; }
        public List<string> tags { get; set; }
        public string title {get;set;}
    }
}
