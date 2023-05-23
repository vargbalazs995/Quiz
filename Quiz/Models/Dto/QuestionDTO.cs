using Quiz.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models.Dto
{
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        [Required]
        [MinLength(5)]
        public string QuizQuestion { get; set; }
        public int Strength { get; set; }
        public string Subject { get; set; }
    }
    public class QuestionCreateDTO
    {
        public string QuizQuestion { get; set; }
        public int Strength { get; set; }
        public string Subject { get; set; }
    }
    public class QuestionUpdateDTO
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        [MinLength(5)]
        public string QuizQuestion { get; set; }
        public int Strength { get; set; }
        public string Subject { get; set; }
    }

    public class QuestionWithAnswDTO
    {
        public int QuestionId { get; set; }
        [Required]
        [MinLength(5)]
        public string QuizQuestion { get; set; }
        public int Strength { get; set; }
        public string Subject { get; set; }
        public List<AnswersDTO> Answers;
    }

    public class QuestAndAnswCreateDTO
    {
        [Required]
        [MinLength(5)]
        public string QuizQuestion { get; set; }
        public int Strength { get; set; }
        public string Subject { get; set; }

        public List<AnswerCreateDTO> AnswersCreate;
    }
}
