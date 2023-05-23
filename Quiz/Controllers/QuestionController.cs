using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;
using Quiz.Models.Dto;
using Quiz.Models.Entities;
using Quiz.Repository.IRepository;
using System.Net;

namespace Quiz.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IQuestionRepository _dbQuestion;
        private readonly IAnswerRepository _dbAnswer; 
        private readonly IMapper _mapper;

           public QuestionController(IQuestionRepository dbQuestion,IAnswerRepository dbAnswer, IMapper mapper ) 
        {
            _dbQuestion = dbQuestion;
            _dbAnswer = dbAnswer;
            _mapper = mapper;
            this._response = new();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetQuestions()
        {
            try
            {
                IEnumerable<QuestionEntity> questionList = await _dbQuestion.GetAllAsync();
                _response.Result = _mapper.Map<List<QuestionDTO>>(questionList);
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

        

        [HttpGet("{id:int}", Name = "GetQuestion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetQuestion(int id)
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
                _response.Result = _mapper.Map<QuestionDTO>(question);
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
        public async Task<ActionResult<APIResponse>> PostQuestion([FromBody] QuestionCreateDTO question)
        {
            try
            {
                if (await _dbQuestion.GetAsync(u => u.QuizQuestion.ToLower() == question.QuizQuestion.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Question already exists!");
                    return BadRequest(ModelState);
                }
                if (question == null)
                {
                    return BadRequest(question);
                }

                QuestionEntity model = _mapper.Map<QuestionEntity>(question);

                await _dbQuestion.CreateAsync(model);
                _response.Result = _mapper.Map<QuestionDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetQuestion", new { id = model.QuestionId}, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

       

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> DeleteQuestion(int id) 
        {
            try {
            if (id == 0)
            {
                return BadRequest();
            }

            var question =await _dbQuestion.GetAsync(u => u.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            await _dbQuestion.RemoveAsync(question);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateQuestion")]
        public async Task<ActionResult<APIResponse>> UpdateQuestion(int id, [FromBody] QuestionUpdateDTO questionDTO)
        {
            try
            {
                if (questionDTO == null || id != questionDTO.QuestionId)
                {
                    return BadRequest();
                }

                QuestionEntity model = _mapper.Map<QuestionEntity>(questionDTO);

                await _dbQuestion.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialQuestion")]
        public async Task<IActionResult> UpdatePartialQuestion(int id, JsonPatchDocument<QuestionUpdateDTO> patchDTO)
        { 
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var question = await _dbQuestion.GetAsync(u => u.QuestionId == id, tracked: false);

            QuestionUpdateDTO questionDTO = _mapper.Map<QuestionUpdateDTO>(question);

            if (question == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(questionDTO, ModelState);

            QuestionEntity model = _mapper.Map<QuestionEntity>(questionDTO);

            await _dbQuestion.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return NoContent();
        }
    }
}
