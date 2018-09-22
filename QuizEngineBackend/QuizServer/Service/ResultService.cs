using MongoDB.Bson;
using QuizServer.Data;
using QuizServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace QuizServer.Service
{
    public class ResultService : IResultService
    {

        private readonly IResultContext _context;

        public ResultService(IResultContext context)
        {
            _context = context;
        }
        public async Task PostUserInfo(UserInfo userInfo) => await _context.ResultCollection.InsertOneAsync(userInfo);

        public async Task<UserInfo> GetByID(int id)
        {
            var userInfo = await _context.ResultCollection.Find(t => t.UserId == id).FirstOrDefaultAsync();

            return userInfo;
        }

        public async Task<List<UserInfo>> GetAll()
        {
            var entries = new List<UserInfo>();
            var allDocuments = await _context.ResultCollection.FindAsync(new BsonDocument());

            await allDocuments.ForEachAsync(doc => entries.Add(doc));

            return entries;
        }
    }
}
