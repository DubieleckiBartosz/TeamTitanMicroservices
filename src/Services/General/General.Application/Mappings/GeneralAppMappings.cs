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
        CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.ToString()));
        CreateMap<Reaction, ReactionViewModel>()
            .ForMember(dest => dest.Reaction, opt => opt.MapFrom(src => (ReactionType) src.Type.Id));
        CreateMap<Attachment, AttachmentViewModel>();
        CreateMap<Post, PostViewModel>()
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Content == null ? null : src.Content.ToString()));
    }
}