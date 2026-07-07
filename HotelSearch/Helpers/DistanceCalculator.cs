using HotelSearch.Models;

namespace HotelSearch.Helpers
{
    public static class DistanceCalculator
    {
        private const double EarthRadiusKm = 6371;

        public static double CalculateDistanceKm(GeoLocation firstLocation, GeoLocation secondLocation)
        {
            double latitudeDistance = ToRadians(secondLocation.Latitude - firstLocation.Latitude);
            double longitudeDistance = ToRadians(secondLocation.Longitude - firstLocation.Longitude);

            double firstLatitude = ToRadians(firstLocation.Latitude);
            double secondLatitude = ToRadians(secondLocation.Latitude);

            double a = Math.Sin(latitudeDistance / 2) * Math.Sin(latitudeDistance / 2)
                + Math.Cos(firstLatitude) * Math.Cos(secondLatitude)
                * Math.Sin(longitudeDistance / 2) * Math.Sin(longitudeDistance / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return Math.Round(EarthRadiusKm * c, 2);
        }

        private static double ToRadians(double value)
        {
            return value * Math.PI / 180;
        }
    }
}