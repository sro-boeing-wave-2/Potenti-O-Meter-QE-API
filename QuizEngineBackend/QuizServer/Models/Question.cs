using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class Question
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string QuestionId { get; set; }

        public string Domain { get; set; }
        public string QuestionText { get; set; }
        public List<Options> OptionList { get; set; }
        public string QuestionType { get; set; }
        public string[] ConceptTags { get; set; }
        public string userResponse { get; set; }
        public int DifficultyLevel { get; set; }
        public bool IsCorrect { get; set; }
        public string CorrectOption{ get; set; }
        //public List<Options> CorrectOption { get; set; }
    }

    public class Options
    {
        public string Option { get; set; }
    }


}
