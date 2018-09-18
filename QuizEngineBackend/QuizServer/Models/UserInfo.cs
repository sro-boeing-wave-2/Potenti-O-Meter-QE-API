using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QuizServer.Service;

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

        public int MaximumDifficaultyLevelReached { get; set; }

        public List<Question> QuestionsAttempted = new List<Question>();

        public List<Question> QuestionBank = new List<Question>();


        //public List<Question> QuestionBank = new List<Question>()
        //{
        //    new Question()
        //    {
        //        QuestionId = "1",
        //        QuestionText = "Who is the CM of WestBengal?",
        //        Options = new List<string>()
        //            {
        //                "Siddu",
        //                "Didi",
        //                "Modi",
        //                "RaGa"
        //            }
        //    },


        //    new Question()
        //    {
        //        QuestionId = "2",
        //        QuestionText = "Who is the CM of Karnataka?",
        //        Options = new List<string>()
        //            {
        //                "deepu",
        //                "shashaidar",
        //                "Deepika",
        //                "deepthi"
        //            }
        //    },

        //    new Question()
        //    {
        //        QuestionId = "3",
        //        QuestionText = "Who is the CM of Kerala?",
        //        Options = new List<string>()
        //            {
        //                "Siddu",
        //                "Didi",
        //                "Modi",
        //                "RaGa"
        //            }
        //    },
        //    new Question()
        //    {
        //        QuestionId = "4",
        //        QuestionText = "Who is the best ----- developer in stack route?",
        //        Options = new List<string>()
        //            {
        //                "deepu",
        //                "deepu",
        //                "deepu",
        //                "FE"
        //            }
        //    }
        //};

        //private IQuizEngineService _quizEngineService;
        //public UserInfo(IQuizEngineService quizEngineService, List<Question> questions)
        //{
        //    _quizEngineService = quizEngineService;
        //    //QuestionBank = _quizEngineService.GetQuestionByDomain();
        //    this.QuestionBank = questions;
        //    this.QuestionsAttempted = questions;
        //}

        //public UserInfo()
        //{
        //}
    }
}
