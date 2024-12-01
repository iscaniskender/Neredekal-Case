using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Application.Hotel.Dto
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonDto> AuthorizedPersons { get; set; } = new();
    }
}

