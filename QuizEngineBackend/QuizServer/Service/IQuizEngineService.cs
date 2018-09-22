using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Potentiometer.Core.QuestionTypes;
using QuizServer.Models;

namespace QuizServer.Service
{
    public interface IQuizEngineService
    {
        Task<List<Object>> GetQuestionByDomain(string domain);
        Task PostUserInfoAsync(UserInfo userInfo);
        Task<Object> GetConceptGraph(string domain);
    }
}
