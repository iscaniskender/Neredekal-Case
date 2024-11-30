using HotelService.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Entity
{
    public class ContactInfoEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public ContactType Type { get; set; }
        public string Content { get; set; }
        public Guid HotelId { get; set; }
        public required HotelEntity Hotel { get; set; }
    }
}
