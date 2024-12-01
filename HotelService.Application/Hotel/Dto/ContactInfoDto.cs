using HotelService.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Application.Hotel.Dto
{
    public class ContactInfoDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public Guid HotelId { get; set; }
    }
}
