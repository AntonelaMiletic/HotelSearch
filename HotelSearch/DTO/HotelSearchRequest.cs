namespace HotelSearch.DTO
{
    public class HotelSearchRequest
    {
        public string Prompt { get; set; } = string.Empty;

        public double CurrentLatitude { get; set; }

        public double CurrentLongitude { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}