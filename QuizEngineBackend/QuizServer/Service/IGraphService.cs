using Neo4j.Driver.V1;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Service
{
    public interface IGraphService
    {
        void GetGraph(string domain);
        IStatementResult CreateConceptToQuestionMapping(List<Triplet> node, string version, string domain);
        IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node);
       
        bool ConceptExists(Concept node);
        bool QuestionIdExists(QuestionIdNode node);
        bool IsDomainExist(string domain);
        void Dispose();
    }
}
