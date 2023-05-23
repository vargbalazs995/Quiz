using Microsoft.EntityFrameworkCore;
using Quiz.ApplicationDbContext;
using Quiz.Models.Entities;
using Quiz.Repository.IRepository;
using System.Linq.Expressions;

namespace Quiz.Repository
{
    public class QuestionRepository :Repository<QuestionEntity>, IQuestionRepository
    {
        private readonly AppDbContext _db;

        public QuestionRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

 
        public async Task<QuestionEntity> UpdateAsync(QuestionEntity entity)
        {
            _db.Questions.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
