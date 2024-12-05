using HotelService.Data.Entity;

namespace HotelService.Data.Repository.ContactInfo
{
    public interface IContactInfoRepository
    {
        
        Task<List<ContactInfoEntity>> GetAllContactInfosAsync();

       
        Task<ContactInfoEntity> GetContactInfoByIdAsync(Guid id);

        
        Task<List<ContactInfoEntity>> GetContactInfosByHotelIdAsync(Guid hotelId);

   
        Task AddContactInfoAsync(ContactInfoEntity contactInfo);

       
        Task UpdateContactInfoAsync(ContactInfoEntity contactInfo);

     
        Task DeleteContactInfoAsync(Guid id);
    }
}
