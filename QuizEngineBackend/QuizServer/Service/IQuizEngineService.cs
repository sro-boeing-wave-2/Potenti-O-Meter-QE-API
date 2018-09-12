using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizServer.Models;

namespace QuizServer.Service
{
    public interface IQuizEngineService
    {

        Task<List<Question>> GetQuestionByDomain(int userId, string domain);
        Task PostUserInfoAsync(UserInfo userInfo);
        //private static Random rng = new Random();
        //void Shuffle<T>(this IList<T> list);
    }
}
