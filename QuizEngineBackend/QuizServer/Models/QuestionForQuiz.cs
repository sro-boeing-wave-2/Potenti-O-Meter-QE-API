using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizServer.Models
{
    public class QuestionForQuiz
    {
        public string QuestionId { get; set; }

        public string Domain { get; set; }
        public string QuestionText { get; set; }
        public List<Option> OptionList { get; set; }
        public string QuestionType { get; set; }
        public string[] ConceptTags { get; set; }
        public string userResponse { get; set; }
        public int DifficultyLevel { get; set; }
        public bool IsCorrect { get; set; }
        //public string CorrectOption { get; set; }
    }

    public class Option
    {
        public string Options { get; set; }
    }

}
