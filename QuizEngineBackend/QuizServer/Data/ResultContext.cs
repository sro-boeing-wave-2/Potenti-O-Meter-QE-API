using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Data
{
    public class ResultContext : IResultContext
    {
        private readonly IMongoDatabase _database;
        public ResultContext(IOptions<Settings> settings)
        {
            try
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                if (client != null)
                    _database = client.GetDatabase(settings.Value.Database);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }

        }

        public IMongoCollection<UserInfo> ResultCollection => _database.GetCollection<UserInfo>("ResultCollection");


    }
}
