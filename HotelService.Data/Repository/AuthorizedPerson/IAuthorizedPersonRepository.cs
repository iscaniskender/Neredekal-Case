using HotelService.Data.Entity;

namespace HotelService.Data.Repository.AuthorizedPerson
{
    public interface IAuthorizedPersonRepository
    {
 
        Task<List<AuthorizedPersonEntity>> GetAllAuthorizedPersonsAsync();

        Task<AuthorizedPersonEntity?> GetAuthorizedPersonByIdAsync(Guid id);

  
        Task<List<AuthorizedPersonEntity>> GetAuthorizedPersonsByHotelIdAsync(Guid hotelId);

        Task AddAuthorizedPersonAsync(AuthorizedPersonEntity authorizedPerson);

     
        Task UpdateAuthorizedPersonAsync(AuthorizedPersonEntity authorizedPerson);

        Task DeleteAuthorizedPersonAsync(Guid id);
    }
}
