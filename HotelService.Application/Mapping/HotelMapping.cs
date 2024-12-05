using AutoMapper;
using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.ContactInfo.Command;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;
using HotelService.Data.Entity;
using HotelService.Data.Enum;

namespace HotelService.Application.Mapping;

public class HotelMapping : Profile
{
    public HotelMapping()
    {
        CreateMap<HotelEntity, HotelDto>().ReverseMap();
        
        CreateMap<CreateHotelCommand,HotelEntity>();
        
        CreateMap<UpdateHotelCommand, HotelEntity>();
        
        CreateMap<CreateAuthorizedPersonCommand, AuthorizedPersonEntity>();
        
        CreateMap<CreateContactInfoCommand, ContactInfoEntity>();
        
        CreateMap<ContactInfoDto, ContactInfoEntity>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<ContactType>(src.Type)))
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        
        CreateMap<AuthorizedPersonDto,AuthorizedPersonEntity>().ReverseMap();
    }
}