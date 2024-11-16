using AutoMapper;
using CandidateHub.DTOs;
using CandidateHub.Models;

namespace CandidateHub.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CandidateDto, Candidate>().ReverseMap();    
        }

    }
}
