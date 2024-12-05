using Microsoft.EntityFrameworkCore;
using HotelService.Data.Context;
using HotelService.Data.Entity;
using HotelService.Data.Repository.ContactInfo;

namespace HotelService.Data.Repository
{
    public class ContactInfoRepository : IContactInfoRepository
    {
        private readonly HotelDbContext _context;

        public ContactInfoRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactInfoEntity>> GetAllContactInfosAsync()
        {
            return await _context.ContactInfos.Where(x=>x.IsActive).AsNoTracking().ToListAsync();
        }

        public async Task<ContactInfoEntity?> GetContactInfoByIdAsync(Guid id)
        {
            return await _context.ContactInfos.FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public async Task<List<ContactInfoEntity>> GetContactInfosByHotelIdAsync(Guid hotelId)
        {
            return await _context.ContactInfos
                .Where(c => c.HotelId == hotelId && c.IsActive)
                .ToListAsync();
        }

        public async Task UpdateContactInfoAsync(ContactInfoEntity contactInfo)
        {
            _context.ContactInfos.Update(contactInfo);
            await _context.SaveChangesAsync();
        }

        public async Task AddContactInfoAsync(ContactInfoEntity contactInfo)
        {
            await _context.ContactInfos.AddAsync(contactInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContactInfoAsync(Guid id)
        {
            var contactInfo = await _context.ContactInfos.Where(x=>x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (contactInfo != null)
            {
                _context.ContactInfos.Remove(contactInfo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
