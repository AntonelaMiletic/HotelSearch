using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSearch.Services;

namespace HotelSearch.Tests
{
    public class SimplePromptParserTests
    {
        [Fact]
        public void Parse_WhenPromptContainsBudget_ReturnsMaxBudget()
        {
            SimplePromptParser parser = new SimplePromptParser();

            var result = parser.Parse("Find hotels in Zagreb under 150");

            Assert.Equal(150m, result.MaxBudget);
        }

        [Fact]
        public void Parse_WhenPromptContainsKnownLocation_ReturnsLocation()
        {
            SimplePromptParser parser = new SimplePromptParser();

            var result = parser.Parse("Find hotels in Zagreb under 150");

            Assert.NotNull(result.Location);
            Assert.Equal(45.8150, result.Location!.Latitude);
            Assert.Equal(15.9819, result.Location.Longitude);
        }

        [Fact]
        public void Parse_WhenPromptDoesNotContainBudget_ReturnsNullBudget()
        {
            SimplePromptParser parser = new SimplePromptParser();

            var result = parser.Parse("Find hotels in Zagreb");

            Assert.Null(result.MaxBudget);
        }

        [Fact]
        public void Parse_WhenPromptDoesNotContainKnownLocation_ReturnsNullLocation()
        {
            SimplePromptParser parser = new SimplePromptParser();

            var result = parser.Parse("Find hotels under 150");

            Assert.Null(result.Location);
        }
    }
}