using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class ConceptMap
    {

        public Concept Source { get; set; }
        public Concept Target { get; set; }
        public Relationship Predicate { get; set; }
    }
}
