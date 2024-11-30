using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Entity
{
    public class AuthorizedPersonEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid HotelId { get; set; }
        public HotelEntity Hotel { get; set; }
    }
}
