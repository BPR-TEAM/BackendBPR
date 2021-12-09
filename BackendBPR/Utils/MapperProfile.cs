using System.Linq;
using AutoMapper;
using BackendBPR.Database;

internal class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Advice, CustomAdvice>()
            .ForMember(advice => advice.Likes, cfg => cfg.MapFrom(
                a => a.UserAdvices.Where( userAdvice => userAdvice.Type == AdviceRole.Like).Count()))
            .ForMember(advice => advice.Dislikes, cfg => cfg.MapFrom(
                a => a.UserAdvices.Where( userAdvice => userAdvice.Type == AdviceRole.Dislike).Count()))            
            .ForMember(advice => advice.CreatorId, cfg => cfg.MapFrom(advice => 
            advice.UserAdvices.FirstOrDefault( userAdvice => userAdvice.Type == AdviceRole.Creator).UserId));
    }
}