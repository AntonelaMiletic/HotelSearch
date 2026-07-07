using HotelSearch.Models;

namespace HotelSearch.Repositories
{
    public class InMemoryHotelRepository : IHotelRepository
    {
        private readonly List<Hotel> _hotels = new();
        private int _nextId = 1;

        public InMemoryHotelRepository()
        {
            Add(new Hotel
            {
                Name = "Hotel Zagreb Center",
                Price = 90,
                Location = new GeoLocation
                {
                    Latitude = 45.8131,
                    Longitude = 15.9820
                }
            });

            Add(new Hotel
            {
                Name = "Hotel Dubrovnik",
                Price = 120,
                Location = new GeoLocation
                {
                    Latitude = 45.8129,
                    Longitude = 15.9771
                }
            });

            Add(new Hotel
            {
                Name = "Hotel Far Away",
                Price = 70,
                Location = new GeoLocation
                {
                    Latitude = 45.9000,
                    Longitude = 16.1000
                }
            });
        }

        public List<Hotel> GetAll()
        {
            return _hotels;
        }

        public Hotel? GetById(int id)
        {
            return _hotels.FirstOrDefault(hotel => hotel.Id == id);
        }

        public Hotel Add(Hotel hotel)
        {
            hotel.Id = _nextId;
            _nextId++;

            _hotels.Add(hotel);

            return hotel;
        }

        public bool Update(Hotel hotel)
        {
            Hotel? existingHotel = GetById(hotel.Id);

            if (existingHotel == null)
                return false;

            existingHotel.Name = hotel.Name;
            existingHotel.Price = hotel.Price;
            existingHotel.Location = hotel.Location;

            return true;
        }

        public bool Delete(int id)
        {
            Hotel? hotel = GetById(id);

            if (hotel == null)
                return false;

            _hotels.Remove(hotel);

            return true;
        }
    }
}