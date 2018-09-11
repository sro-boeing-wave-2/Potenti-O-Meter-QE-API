using MongoDB.Driver;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Data
{
    public interface IResultContext
    {
        IMongoCollection<UserInfo> ResultCollection { get; }
    }
}
