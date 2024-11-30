using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Data.Entity
{
    public class HotelEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ContactInfoEntity> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonEntity> AuthorizedPersons { get; set; } = new();
    }
}
