using Quiz.Models.Enums;

namespace Quiz.Models.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }   
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
