using HotelService.Data.Entity;

namespace HotelService.Data.Repository.Hotel
{
    public interface IHotelRepository
    {
        Task<HotelEntity[]> GetAllHotelsAsync();
        Task<HotelEntity> GetHotelByIdAsync(Guid id);
        Task AddHotelAsync(HotelEntity hotel);
        Task UpdateHotelAsync(HotelEntity hotel);
        Task DeleteHotelAsync(Guid id);
        Task<HotelEntity[]> GetHotelsByConditionAsync(string name);
    }
}
