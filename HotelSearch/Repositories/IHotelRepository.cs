using HotelSearch.Models;

namespace HotelSearch.Repositories
{
    public interface IHotelRepository
    {
        List<Hotel> GetAll();

        Hotel? GetById(int id);

        Hotel Add(Hotel hotel);

        bool Update(Hotel hotel);

        bool Delete(int id);
    }
}