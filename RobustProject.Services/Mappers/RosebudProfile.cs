using AutoMapper;
using RobustProject.Services.Entities;
using RobustProject.Services.Models;

namespace RobustProject.Services.Mappers;

public class RosebudProfile : Profile
{
    public RosebudProfile()
    {
        CreateMap<RosebudModel, Rosebud>().ReverseMap();
    }
}