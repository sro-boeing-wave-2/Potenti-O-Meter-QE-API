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
       
        IStatementResult CreateConceptToQuestionMapping(List<Triplet> node, string version, string domain);
        IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node);
        void UpdateUserConceptRelation(string questionId, int userId);
        void UpdateUserConceptRelationForWrongQuestion(string questionId, int userId);
        IStatementResult GetQuestionsFromGraph(int UserId, string DomainName);
        bool ConceptExists(Concept node);
        bool QuestionIdExists(QuestionIdNode node);
        bool IsDomainExist(string domain);
        bool IsUserExist(int userId);
        void CreateUser(int userId);
        void Dispose();
    }
}
