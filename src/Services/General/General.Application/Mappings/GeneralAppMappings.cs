using AutoMapper;
using General.Application.Models.Enums;
using General.Application.Models.ViewModels;
using General.Domain.Entities;
using General.Domain.ValueObjects;

namespace General.Application.Mappings;

public class GeneralAppMappings : Profile
{
    public GeneralAppMappings()
    {
        this.CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()));
        this.CreateMap<Reaction, ReactionViewModel>()
            .ForMember(dest => dest.Reaction, opt => opt.MapFrom(src => (ReactionType) src.Type.Id));
    }
}