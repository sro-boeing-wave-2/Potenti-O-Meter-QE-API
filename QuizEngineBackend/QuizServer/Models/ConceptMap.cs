using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class ConceptMap
    {

        public Concept source { get; set; }
        public Concept target { get; set; }
        public Relationship relationship { get; set; }
    }
}
