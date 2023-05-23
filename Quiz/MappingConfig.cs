using AutoMapper;
using Quiz.Models;
using Quiz.Models.Dto;
using Quiz.Models.Entities;

namespace Quiz
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<QuestionEntity, QuestionDTO>().ReverseMap();
            CreateMap<QuestionEntity, QuestionCreateDTO>().ReverseMap();
            CreateMap<QuestionEntity, QuestionUpdateDTO>().ReverseMap();
            CreateMap<AnswerEntity, AnswerDTO>().ReverseMap();
            CreateMap<AnswerEntity, AnswerCreateDTO>().ReverseMap();
            CreateMap<AnswerEntity, AnswerUpdateDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
