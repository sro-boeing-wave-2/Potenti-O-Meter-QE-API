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
        IStatementResult CreateConceptToConceptMapping(List<ConceptMap> node, string domain);
        IStatementResult CreateContentRecommendationMapping(List<ContentConceptTriplet> node, string domain);
        List<string> GetQuestionsFromGraph(int UserId, string DomainName);
        bool ConceptExists(Concept node);
        bool QuestionIdExists(QuestionIdNode node);
        bool IsDomainExist(string domain);
        bool IsUserExist(int userId);
        void CreateUser(int userId);
        void Dispose();
        void UpdateUserConceptRelation(UserInfo userInfo, int userId);
        List<ContentRecommender> GetContentRecommendations(int userId, string domain);
        string GetBestDomain(int userID)
    }
}
