using AutoMapper;
using LSTM.Models.DTOs;
using LSTM.Models;

namespace LSTM.UI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // 映射现有的类
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<SentimentAnalysis, SentimentAnalysisResultDto>().ReverseMap();
            // 后续可以添加更多映射
        }
    }
} 