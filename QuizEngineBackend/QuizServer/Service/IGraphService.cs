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
        IStatementResult GetGraph();
        IStatementResult CreateConceptNode(List<Triplet> node);
        IStatementResult CreateQuestionIdNode(QuestionIdNode node);
        bool ConceptExists(Concept node);
        bool QuestionIdExists(QuestionIdNode node);
        IStatementResult GetConceptwithRelationships(string nodename);
        IStatementResult CreateConceptwithAssociatedConcepts(ConceptMap concepts);
        void Dispose();
    }
}
