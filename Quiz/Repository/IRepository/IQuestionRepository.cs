using Quiz.Models.Entities;
using System.Linq.Expressions;

namespace Quiz.Repository.IRepository
{
    public interface IQuestionRepository : IRepository<QuestionEntity>
    {
        Task<QuestionEntity> UpdateAsync(QuestionEntity entity);
    }
}
