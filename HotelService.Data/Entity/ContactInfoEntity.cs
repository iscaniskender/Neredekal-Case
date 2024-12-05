using HotelService.Data.Enum;
using App.Core.BaseClass;

namespace HotelService.Data.Entity
{
    public class ContactInfoEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public ContactType Type { get; set; }
        public string Content { get; set; }
        public Guid HotelId { get; set; }
        public HotelEntity Hotel { get; set; }
    }
}
