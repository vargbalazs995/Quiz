using Quiz.ApplicationDbContext;
using Quiz.Models.Entities;
using Quiz.Repository.IRepository;

namespace Quiz.Repository
{
    public class AnswerRepository : Repository<AnswerEntity>, IAnswerRepository
    {
        private readonly AppDbContext _db;

        public AnswerRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<AnswerEntity> UpdateAsync(AnswerEntity entity)
        {
            _db.Answers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
