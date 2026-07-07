using HotelSearch.Models;

namespace HotelSearch.Services
{
    public interface IPromptParser
    {
        SearchParameters Parse(string prompt);
    }
}