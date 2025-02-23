using AutoMapper;
using RobustProject.Services.Models;
using RobustProject.Services.Repository;

namespace RobustProject.Services.Mappers;

public class RosebudProfile : Profile
{
    public RosebudProfile()
    {
        CreateMap<RosebudModel, Rosebud>().ReverseMap();
    }
}