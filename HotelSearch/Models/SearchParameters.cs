namespace HotelSearch.Models
{
    public class SearchParameters
    {
        public decimal? MaxBudget { get; set; }

        public GeoLocation? Location { get; set; }
    }
}