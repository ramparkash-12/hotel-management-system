using AutoMapper;
using Booking.API.Migrations;

namespace Booking.API.Helpers
{
    public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
        /*CreateMap<Room, RoomForDetailsDto>()
            .ForMember(dest => dest.RoomTypeDescription, opt =>  {
                opt.MapFrom(src => src.RoomType.Description);
            })
            .ForMember(dest => dest.RoomTypeName, opt =>  {
                opt.MapFrom(src => src.RoomType.Name);
            })
            .ForMember(dest => dest.RoomTypePrice, opt =>  {
                opt.MapFrom(src => src.RoomType.price);
            });
        */

          CreateMap<Model.Dtos.BookingDto, Model.Booking>();
          CreateMap<Model.Dtos.BookingDto, Model.PaymentMethod>()
          .ForMember(dest => dest.SecurityNumber, opt =>  {
                opt.MapFrom(src => src.cvv);
            })
            .ForMember(dest => dest.CardNumber, opt =>  {
                opt.MapFrom(src => src.CardNumber);
            })
            .ForMember(dest => dest.Expiry, opt =>  {
                opt.MapFrom(src => src.ExpiryMM + "/" + src.ExpiryYY);
            });
        CreateMap<Model.Dtos.BookingDto, Model.BookingPayment>()
        .ForMember(dest => dest.Amount, opt =>  {
            opt.MapFrom(src => src.TotalFee);
        });
    }
  }
}