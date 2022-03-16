using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.IRepository;
using HotelListing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.API.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var results = _mapper.Map<List<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(GetHotels)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(GetHotel)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetHotel", new {id = hotel.Id},hotel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int  id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if(!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => id == q.Id);

                if(hotel is null)
                {
                    _logger.LogError($"Invalid Update attempt in {nameof(UpdateHotel)}. Record not found for {id}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(hotelDTO, hotel);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something went wrong in {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if(id < 1)
            {
                _logger.LogError("Invalid Delete attempt in {method} with id {id}", nameof(DeleteHotel), id);
                return BadRequest("Invalid id supply");
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id.Equals(id));

                if(hotel is null)
                {
                    _logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}. Record not found for {id}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();

                return Ok("Record Deleted successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }
}
