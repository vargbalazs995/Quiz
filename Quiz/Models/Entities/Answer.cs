using System.ComponentModel.DataAnnotations;

namespace Quiz.Models.Entities
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string AnswerOfQuestion { get; set; }
        public int QuestionId { get; set;}
        public Question Question { get; set; }
    }
}
