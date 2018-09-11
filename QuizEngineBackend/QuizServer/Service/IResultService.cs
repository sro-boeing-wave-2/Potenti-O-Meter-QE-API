using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Service
{
    public interface IResultService
    {
        Task PostUserInfo(UserInfo userInfo);
        Task<UserInfo> GetByID(int id);
        Task<List<UserInfo>> GetAll();
    }
}
