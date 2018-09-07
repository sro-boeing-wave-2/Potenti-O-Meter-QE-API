using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizServer.Models;

namespace QuizServer.Service
{
    public interface IQuizEngineService
    {
       
            Task GetQuestionByDomain();
            Task PostUserInfoAsync(UserInfo userinfo);
        
    }
}
