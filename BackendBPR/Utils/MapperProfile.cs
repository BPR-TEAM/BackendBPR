using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BackendBPR.ApiModels;
using BackendBPR.Database;

internal class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Advice, AdviceExtendedApi>()
            .ForMember(advice => advice.Likes, cfg => cfg.MapFrom(
                a => a.UserAdvices.Where( userAdvice => userAdvice.Type == AdviceRole.Like).Count()))
            .ForMember(advice => advice.Dislikes, cfg => cfg.MapFrom(
                a => a.UserAdvices.Where( userAdvice => userAdvice.Type == AdviceRole.Dislike).Count()))
            .ForMember(advice => advice.CreatorName, cfg => cfg.MapFrom(
                (a,c) => {var user = a.UserAdvices.FirstOrDefault(u => u.Type == AdviceRole.Creator).User;
                    return user != null ? user.Username : "";}))
            .ForMember(advice => advice.CreatorImage, cfg => cfg.MapFrom(
                (a,c) => {var user = a.UserAdvices.FirstOrDefault(u => u.Type == AdviceRole.Creator).User;
                return user != null ? user.Image : new byte[0];}));
        CreateMap<Advice,GiveAdviceApi>().ReverseMap();

        CreateMap<RegisterUserApi,User>();

        CreateMap<CreateDashboardApi, Dashboard>().ForMember( create => create.UserPlants, cfg => cfg.MapFrom(
            (a,b) => { 
                return new List<UserPlant>();}
                ));

        CreateMap<UserPlantApi, UserPlant>();

        CreateMap<CustomMeasurementDefinitionApi, CustomMeasurementDefinition>();

        CreateMap<NoteApi, Note>();

        CreateMap<BoardApi, Board>();

    }
}