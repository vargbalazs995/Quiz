using Quiz.Models.Entities;

namespace Quiz.Repository.IRepository
{
    public interface IAnswerRepository : IRepository<AnswerEntity>
    {
        Task<AnswerEntity> UpdateAsync(AnswerEntity entity);
    }
}
