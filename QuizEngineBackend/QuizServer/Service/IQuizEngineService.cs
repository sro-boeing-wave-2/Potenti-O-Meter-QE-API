using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizServer.Models;

namespace QuizServer.Service
{
    public interface IQuizEngineService
    {

        Task<List<Question>> GetQuestionByDomain(string domain);
        Task PostUserInfoAsync(UserInfo userInfo);
       
    }
}
