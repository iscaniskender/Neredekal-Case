﻿using MediatR;
using App.Core.Results;
using HotelService.Application.Dto;

namespace HotelService.Application.Hotel.Command
{
    public class CreateHotelCommand : IRequest<Result<Unit>>
    {
        public string Name { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonDto> AuthorizedPersons { get; set; } = new();
    }
}
