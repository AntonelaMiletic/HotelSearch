using System.Text.RegularExpressions;
using HotelSearch.Models;

namespace HotelSearch.Services
{
    public class SimplePromptParser : IPromptParser
    {
        private readonly Dictionary<string, GeoLocation> _knownLocations = new()
        {
            {
                "zagreb",
                new GeoLocation
                {
                    Latitude = 45.8150,
                    Longitude = 15.9819
                }
            },
            {
                "split",
                new GeoLocation
                {
                    Latitude = 43.5081,
                    Longitude = 16.4402
                }
            },
            {
                "rijeka",
                new GeoLocation
                {
                    Latitude = 45.3271,
                    Longitude = 14.4422
                }
            },
            {
                "oslo",
                new GeoLocation
                {
                    Latitude = 59.9139,
                    Longitude = 10.7522
                }
            }
        };

        public SearchParameters Parse(string prompt)
        {
            SearchParameters parameters = new SearchParameters();

            if (string.IsNullOrWhiteSpace(prompt))
                return parameters;

            parameters.MaxBudget = ExtractBudget(prompt);
            parameters.Location = ExtractLocation(prompt);

            return parameters;
        }

        private decimal? ExtractBudget(string prompt)
        {
            Match match = Regex.Match(
                prompt,
                @"(under|below|max|maximum|up to)\s+(\d+)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            bool isParsed = decimal.TryParse(match.Groups[2].Value, out decimal budget);

            if (!isParsed)
                return null;

            return budget;
        }

        private GeoLocation? ExtractLocation(string prompt)
        {
            string lowerPrompt = prompt.ToLower();

            foreach (var knownLocation in _knownLocations)
            {
                if (lowerPrompt.Contains(knownLocation.Key))
                    return knownLocation.Value;
            }

            return null;
        }
    }
}