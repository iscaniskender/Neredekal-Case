using Microsoft.EntityFrameworkCore;
using HotelService.Data.Context;
using HotelService.Data.Entity;
using HotelService.Data.Enum;

namespace HotelService.Data.Repository.Hotel
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDbContext _context;

        public HotelRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<HotelEntity[]> GetAllHotelsAsync()
        {
            return await _context.Hotels.Where(x=>x.IsActive)
                .Include(h => h.ContactInfos)
                .Include(h => h.AuthorizedPersons)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<HotelEntity?> GetHotelByIdAsync(Guid id)
        {
            return await _context.Hotels
                .Include(h => h.ContactInfos)
                .Include(h => h.AuthorizedPersons)
                .FirstOrDefaultAsync(h => h.Id == id && h.IsActive);
        }

        public async Task AddHotelAsync(HotelEntity hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHotelAsync(HotelEntity hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHotelAsync(Guid id)
        {
            var hotel =await _context.Hotels.Where(x=>x.IsActive && x.Id==id).FirstOrDefaultAsync();
            if (hotel != null)
            {
                _context.Hotels.Remove(
                    hotel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<HotelEntity[]> GetHotelsByConditionAsync(string name)
        {
            var data = _context.Hotels
                .Include(x => x.AuthorizedPersons)
                .Include(y => y.ContactInfos)
                .Where(h => h.ContactInfos
                    .Any(c => c.Type == ContactType.Location && EF.Functions.Like(c.Content.ToLower(), $"%{name.ToLower()}%")));

            return await data.ToArrayAsync();
        }
    }
}
