using HotelSearch.DTO;
using HotelSearch.Helpers;
using HotelSearch.Models;
using HotelSearch.Repositories;

namespace HotelSearch.Services
{
    public class HotelSearchService : IHotelSearchService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IPromptParser _promptParser;

        public HotelSearchService(IHotelRepository hotelRepository, IPromptParser promptParser)
        {
            _hotelRepository = hotelRepository;
            _promptParser = promptParser;
        }

        public PagedResult<HotelSearchResult> Search(HotelSearchRequest request)
        {
            SearchParameters parameters = _promptParser.Parse(request.Prompt);

            GeoLocation currentLocation = parameters.Location ?? new GeoLocation
            {
                Latitude = request.CurrentLatitude,
                Longitude = request.CurrentLongitude
            };

            List<HotelSearchResult> hotels = _hotelRepository.GetAll()
                .Where(hotel => parameters.MaxBudget == null || hotel.Price <= parameters.MaxBudget)
                .Select(hotel => new HotelSearchResult
                {
                    Name = hotel.Name,
                    Price = hotel.Price,
                    DistanceKm = DistanceCalculator.CalculateDistanceKm(currentLocation, hotel.Location)
                })
                .OrderBy(hotel => CalculateSearchScore(hotel.Price, hotel.DistanceKm))
                .ToList();

            int totalCount = hotels.Count;

            List<HotelSearchResult> pagedHotels = hotels
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<HotelSearchResult>
            {
                Items = pagedHotels,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        private decimal CalculateSearchScore(decimal price, double distanceKm)
        {
            return price + (decimal)(distanceKm * 10);
        }
    }
}