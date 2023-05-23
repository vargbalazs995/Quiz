using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using Quiz.Models.Dto;
using Quiz.Models.Entities;
using Quiz.Repository.IRepository;
using System.Net;

namespace Quiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAndAnswerController : ControllerBase
    { 
        private readonly APIResponse _response;
        private readonly IQuestionRepository _dbQuestion;
        private readonly IAnswerRepository _dbAnswer;
        private readonly IMapper _mapper;
        public QuestionAndAnswerController(IQuestionRepository dbQuestion, IAnswerRepository dbAnswer, IMapper mapper)
        {
            _dbAnswer = dbAnswer;
            _dbQuestion = dbQuestion;
            _mapper = mapper;
            this._response = new();
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponse>> GetQuestionsWithAnsw(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var question = await _dbQuestion.GetAsync(u => u.QuestionId == id);

                if (question == null)
                {
                    return NotFound();
                }

                IEnumerable<AnswerEntity> questionList = await _dbAnswer.GetAllAsync(u => u.QuestID == id);

                List<AnswersDTO> answerList = new List<AnswersDTO>();

                foreach (var item in questionList)
                {
                    AnswersDTO value = new()
                    {
                        AnswerId= item.AnswerId,
                        Answer = item.Answer,
                        Correct = item.Correct,
                        QuestID = item.QuestID,
                    };

                    answerList.Add(value);
                }

                QuestionWithAnswDTO result = new()
                {
                    QuestionId = question.QuestionId,
                    QuizQuestion = question.QuizQuestion,
                    Strength = question.Strength,
                    Subject = question.Subject,
                    Answers = answerList
                };

                _response.Result = result;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> PostQNA([FromBody] QuestAndAnswCreateDTO qNdADTO)
        {
            try
            {
                if (await _dbQuestion.GetAsync(u => u.QuizQuestion.ToLower() == qNdADTO.QuizQuestion.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Question already exists!");
                    return BadRequest(ModelState);
                }
                if (qNdADTO == null)
                {
                    return BadRequest(qNdADTO);
                }

                QuestionEntity model = new()
                {
                    QuizQuestion = qNdADTO.QuizQuestion,
                    Strength = qNdADTO.Strength,
                    Subject = qNdADTO.Subject
                };

                await _dbQuestion.CreateAsync(model);
                _response.Result = _mapper.Map<QuestionDTO>(model);

                foreach (var item in qNdADTO.AnswersCreate)
                {
                    AnswerEntity value = new()
                    {
                        Answer = item.Answer,
                        Correct = item.Correct,
                        QuestID = model.QuestionId
                    };
                    await _dbAnswer.CreateAsync(value);
                }
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetQuestion", new { id = model.QuestionId }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
