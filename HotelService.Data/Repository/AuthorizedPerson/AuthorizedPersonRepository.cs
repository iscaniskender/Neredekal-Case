using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HotelService.Data.Context;
using HotelService.Data.Entity;
using HotelService.Data.Repository.AuthorizedPerson;

namespace HotelService.Data.Repository
{
    public class AuthorizedPersonRepository : IAuthorizedPersonRepository
    {
        private readonly HotelDbContext _context;

        public AuthorizedPersonRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorizedPersonEntity>> GetAllAuthorizedPersonsAsync()
        {
            return await _context.AuthorizedPersons.AsNoTracking().ToListAsync();
        }

        public async Task<AuthorizedPersonEntity?> GetAuthorizedPersonByIdAsync(Guid id)
        {
            return await _context.AuthorizedPersons.FirstOrDefaultAsync(x=> x.Id ==id && x.IsActive);
        }

        public async Task<List<AuthorizedPersonEntity>> GetAuthorizedPersonsByHotelIdAsync(Guid hotelId)
        {
            return await _context.AuthorizedPersons
                .Where(a => a.HotelId == hotelId && a.IsActive)
                .ToListAsync();
        }

        public async Task AddAuthorizedPersonAsync(AuthorizedPersonEntity authorizedPerson)
        {
            await _context.AuthorizedPersons.AddAsync(authorizedPerson);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorizedPersonAsync(AuthorizedPersonEntity authorizedPerson)
        {
            _context.AuthorizedPersons.Update(authorizedPerson);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorizedPersonAsync(Guid id)
        {
            var person = await _context.AuthorizedPersons.Where(x=>x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (person != null)
            {
                _context.AuthorizedPersons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}
