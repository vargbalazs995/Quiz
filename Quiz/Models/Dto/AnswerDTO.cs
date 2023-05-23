using Quiz.Models.Entities;

namespace Quiz.Models.Dto
{
    public class AnswerDTO
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public int QuestID { get; set; }
    }
    public class AnswerCreateDTO
    {
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public int QuestID { get; set; }
    }
    public class AnswerUpdateDTO
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public int QuestID { get; set; }
    }

    public class AnswersDTO
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public int QuestID { get; set; }
    }
}

