using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelSearch.DTO;
using HotelSearch.Repositories;
using HotelSearch.Services;

namespace HotelSearch.Tests
{
    public class HotelSearchServiceTests
    {
        [Fact]
        public void Search_WhenBudgetIsSet_ReturnsOnlyHotelsWithinBudget()
        {
            InMemoryHotelRepository repository = new InMemoryHotelRepository();
            SimplePromptParser parser = new SimplePromptParser();
            HotelSearchService service = new HotelSearchService(repository, parser);

            HotelSearchRequest request = new HotelSearchRequest
            {
                Prompt = "Find hotels in Zagreb under 100",
                CurrentLatitude = 45.8150,
                CurrentLongitude = 15.9819,
                Page = 1,
                PageSize = 10
            };

            PagedResult<HotelSearchResult> result = service.Search(request);

            Assert.All(result.Items, hotel => Assert.True(hotel.Price <= 100));
        }

        [Fact]
        public void Search_WhenPageSizeIsTwo_ReturnsTwoItems()
        {
            InMemoryHotelRepository repository = new InMemoryHotelRepository();
            SimplePromptParser parser = new SimplePromptParser();
            HotelSearchService service = new HotelSearchService(repository, parser);

            HotelSearchRequest request = new HotelSearchRequest
            {
                Prompt = "Find hotels in Zagreb under 150",
                CurrentLatitude = 45.8150,
                CurrentLongitude = 15.9819,
                Page = 1,
                PageSize = 2
            };

            PagedResult<HotelSearchResult> result = service.Search(request);

            Assert.Equal(2, result.Items.Count);
            Assert.Equal(3, result.TotalCount);
        }

        [Fact]
        public void Search_WhenHotelsAreReturned_OrdersHotelsByPriceAndDistance()
        {
            InMemoryHotelRepository repository = new InMemoryHotelRepository();
            SimplePromptParser parser = new SimplePromptParser();
            HotelSearchService service = new HotelSearchService(repository, parser);

            HotelSearchRequest request = new HotelSearchRequest
            {
                Prompt = "Find hotels in Zagreb under 150",
                CurrentLatitude = 45.8150,
                CurrentLongitude = 15.9819,
                Page = 1,
                PageSize = 10
            };

            PagedResult<HotelSearchResult> result = service.Search(request);

            Assert.Equal("Hotel Zagreb Center", result.Items[0].Name);
        }
    }
}
