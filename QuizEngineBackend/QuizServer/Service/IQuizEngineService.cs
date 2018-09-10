using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizServer.Models;

namespace QuizServer.Service
{
    public interface IQuizEngineService
    {

        Task<List<Question>> GetQuestionByDomain();
        Task PostUserInfoAsync(UserInfo userinfo);
        //private static Random rng = new Random();
        //void Shuffle<T>(this IList<T> list);
    }
}
