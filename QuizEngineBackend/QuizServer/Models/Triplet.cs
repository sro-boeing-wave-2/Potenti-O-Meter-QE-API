using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class Triplet
    {
        public Concept  source { get; set; }
        public QuestionIdNode target { get; set; }
        public Relationship relationship { get; set; }
    }
}
