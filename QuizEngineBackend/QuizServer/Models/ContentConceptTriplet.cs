using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class ContentConceptTriplet
    {
        public Content Source { get; set; }
        public Concept Target { get; set; }
        public ContentRelationship Relationship { get; set; }
    }
}
