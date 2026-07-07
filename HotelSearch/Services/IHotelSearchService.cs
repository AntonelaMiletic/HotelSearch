using HotelSearch.DTO;

namespace HotelSearch.Services
{
    public interface IHotelSearchService
    {
        PagedResult<HotelSearchResult> Search(HotelSearchRequest request);
    }
}