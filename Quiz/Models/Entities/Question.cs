using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Quiz.Models.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Answer> Answers { get; } = new List<Answer>();
    }
}
