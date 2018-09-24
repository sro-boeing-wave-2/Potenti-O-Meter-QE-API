using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class ConceptGraphWithVersion
    {
        
            public double version { get; set; }
            public string domain { get; set; }
            public List<Triplet> questionconceptTriplet { get; set; }
            public List<ConceptMap> concepttriplet { get; set; }
        
    }
}
