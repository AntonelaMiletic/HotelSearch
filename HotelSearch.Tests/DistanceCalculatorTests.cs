using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSearch.Helpers;
using HotelSearch.Models;

namespace HotelSearch.Tests
{
    public class DistanceCalculatorTests
    {
        [Fact]
        public void CalculateDistanceKm_WhenLocationsAreSame_ReturnsZero()
        {
            GeoLocation location = new GeoLocation
            {
                Latitude = 45.8150,
                Longitude = 15.9819
            };

            double distance = DistanceCalculator.CalculateDistanceKm(location, location);

            Assert.Equal(0, distance);
        }

        [Fact]
        public void CalculateDistanceKm_WhenLocationsAreDifferent_ReturnsPositiveDistance()
        {
            GeoLocation firstLocation = new GeoLocation
            {
                Latitude = 45.8150,
                Longitude = 15.9819
            };

            GeoLocation secondLocation = new GeoLocation
            {
                Latitude = 45.8129,
                Longitude = 15.9771
            };

            double distance = DistanceCalculator.CalculateDistanceKm(firstLocation, secondLocation);

            Assert.True(distance > 0);
        }
    }
}
