using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuizServer.Service;
using QuizServer.Models;
using Potentiometer.Core.QuestionTypes;

namespace QuizServer.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuizId { get; set; }

        public string DomainName { get; set; }

        public int CurrentQuestionIndex { get; set; }

        //public int MaximumDifficaultyLevelReached { get; set; }
        public DateTime Date { get { return DateTime.Now; } }

        public List<Object> QuestionsAttempted = new List<Object>();
        
        public List<Object> QuestionsFromQuestionBank = new List<Object>();
       

        }
        
    }        

           

      
    

