using AutoMapper;
using Hotel.API.Model;
using Hotel.API.Model.Dtos;

namespace Hotel.API.Helpers
{
    public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
        CreateMap<Room, RoomForDetailsDto>()
            .ForMember(dest => dest.RoomTypeDescription, opt =>  {
                opt.MapFrom(src => src.RoomType.Description);
            })
            .ForMember(dest => dest.RoomTypeName, opt =>  {
                opt.MapFrom(src => src.RoomType.Name);
            })
            .ForMember(dest => dest.RoomTypePrice, opt =>  {
                opt.MapFrom(src => src.RoomType.price);
            });

            CreateMap<RoomFacilities, RoomFacilitiesForDetailedDto>();
            CreateMap<Room, RoomForListDto>();
    }
  }
}