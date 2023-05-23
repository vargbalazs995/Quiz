using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz.ApplicationDbContext;
using Quiz.Models;
using Quiz.Models.Dto;
using Quiz.Models.Entities;
using Quiz.Repository.IRepository;
using System.Net;

namespace Quiz.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IAnswerRepository _dbAnswer;
        private readonly IMapper _mapper;

           public AnswerController(IAnswerRepository dbAnswer, IMapper mapper ) 
        {
            _mapper = mapper;
            _dbAnswer = dbAnswer;
            this._response = new();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAnswers()
        {
            try
            {
                IEnumerable<AnswerEntity> answerList = await _dbAnswer.GetAllAsync();
                _response.Result = _mapper.Map<List<AnswerDTO>>(answerList);
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

        [HttpGet("{id:int}", Name = "GetAnswer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task< ActionResult<APIResponse>> GetAnswer(int id)
        {
            try 
            {
            if (id == 0)
            {
                return BadRequest();
            }
            var answer =await _dbAnswer.GetAsync(u => u.AnswerId == id);

            if (answer == null)
            {
                return NotFound();
            }
            _response.Result = _mapper.Map<AnswerDTO>(answer);
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
        public async Task<ActionResult<APIResponse>> PostAnswer([FromBody] AnswerCreateDTO answer)
        {
            try { 
            if (await _dbAnswer.GetAsync(u => u.Answer.ToLower() == answer.Answer.ToLower()) !=null) 
            {
                ModelState.AddModelError("CustomError", "Name already exists!");
                return BadRequest(ModelState);
            }
            if (answer == null)
            {
                return BadRequest(answer);
            }

            AnswerEntity model = _mapper.Map<AnswerEntity>(answer);

           await _dbAnswer.CreateAsync(model);
            _response.Result = _mapper.Map<AnswerDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetAnswer", new { id = model.AnswerId   }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> DeleteAnswer(int id) 
        {
            try {
            if (id == 0)
            {
                return BadRequest();
            }

            var answer =await _dbAnswer.GetAsync(u => u.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }
            await _dbAnswer.RemoveAsync(answer);
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

        [HttpPut("{id:int}", Name = "UpdateAnswer")]
        public async Task<ActionResult<APIResponse>> UpdateAnswer(int id, [FromBody] AnswerUpdateDTO answerDTO)
        {
            try
            {
                if (answerDTO == null || id != answerDTO.AnswerId)
                {
                    return BadRequest();
                }

                AnswerEntity model = _mapper.Map<AnswerEntity>(answerDTO);

                await _dbAnswer.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialAnswer")]
        public async Task<IActionResult> UpdatePartialAnswer(int id, JsonPatchDocument<AnswerUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var answer = await _dbAnswer.GetAsync(u => u.AnswerId == id, tracked: false);

            AnswerUpdateDTO answerDTO = _mapper.Map<AnswerUpdateDTO>(answer);

            if (answer == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(answerDTO, ModelState);

            AnswerEntity model = _mapper.Map<AnswerEntity>(answer);

            await _dbAnswer.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return NoContent();
        }
    }
}
