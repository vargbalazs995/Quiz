using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models.Entities
{
    public class AnswerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        [ForeignKey("Question")]
        public int QuestID { get; set; }
        public QuestionEntity Question { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }

    }
}
