namespace HotelSearch.DTO
{
    public class HotelSearchResult
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public double DistanceKm { get; set; }
    }
}