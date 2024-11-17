using AutoMapper;
using CandidateHub.DTOs;
using CandidateHub.Models;

namespace CandidateHub.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateCandidateDto, Candidate>();
            CreateMap<Candidate, CandidateDto>();
            CreateMap<Candidate, CreateCandidateDto>().ReverseMap();
        }

    }
}
