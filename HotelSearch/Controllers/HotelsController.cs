using HotelSearch.DTO;
using HotelSearch.Models;
using HotelSearch.Repositories;
using HotelSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelSearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelSearchService _hotelSearchService;

        public HotelsController(IHotelRepository hotelRepository, IHotelSearchService hotelSearchService)
        {
            _hotelRepository = hotelRepository;
            _hotelSearchService = hotelSearchService;
        }

        [HttpGet]
        public ActionResult<List<Hotel>> GetAll()
        {
            return Ok(_hotelRepository.GetAll());
        }

        [HttpPost("search")]
        public ActionResult<PagedResult<HotelSearchResult>> Search(HotelSearchRequest request)
        {
            string? validationError = ValidateSearchRequest(request);

            if (validationError != null)
                return BadRequest(validationError);

            PagedResult<HotelSearchResult> result = _hotelSearchService.Search(request);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<Hotel> GetById(int id)
        {
            Hotel? hotel = _hotelRepository.GetById(id);

            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        [HttpPost]
        public ActionResult<Hotel> Create(CreateHotelRequest request)
        {
            if (!IsValidHotelRequest(request.Name, request.Price, request.Latitude, request.Longitude))
                return BadRequest("Hotel data is not valid.");

            Hotel hotel = new Hotel
            {
                Name = request.Name,
                Price = request.Price,
                Location = new GeoLocation
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                }
            };

            Hotel createdHotel = _hotelRepository.Add(hotel);

            return CreatedAtAction(nameof(GetById), new { id = createdHotel.Id }, createdHotel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateHotelRequest request)
        {
            if (!IsValidHotelRequest(request.Name, request.Price, request.Latitude, request.Longitude))
                return BadRequest("Hotel data is not valid.");

            Hotel hotel = new Hotel
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                Location = new GeoLocation
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude
                }
            };

            bool isUpdated = _hotelRepository.Update(hotel);

            if (!isUpdated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _hotelRepository.Delete(id);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

        private bool IsValidHotelRequest(string name, decimal price, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (price <= 0)
                return false;

            if (latitude < -90 || latitude > 90)
                return false;

            if (longitude < -180 || longitude > 180)
                return false;

            return true;
        }

        private string? ValidateSearchRequest(HotelSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return "Prompt is required.";

            if (request.CurrentLatitude < -90 || request.CurrentLatitude > 90)
                return "Current latitude is not valid.";

            if (request.CurrentLongitude < -180 || request.CurrentLongitude > 180)
                return "Current longitude is not valid.";

            if (request.Page <= 0)
                return "Page must be greater than zero.";

            if (request.PageSize <= 0)
                return "Page size must be greater than zero.";

            return null;
        }
    }
}