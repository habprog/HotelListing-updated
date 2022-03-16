using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.IRepository;
using HotelListing.API.Models;
using HotelListing.API.Repository;
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
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly  ILogger _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unityOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {

            try
            {
                var countries = await _unityOfWork.Countries.GetAll();
                var results = _mapper.Map<List<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Ïnternal Server LogError. Please Try Again Later.");
            }
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCounty(int id)
        {
            _logger.LogInformation("Getting country by id {id}", id);
            try
            {
                var country = await _unityOfWork.Countries.Get(q => 
                    q.Id == id, 
                    new List<string> { "Hotels" });

                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Ïnternal Server LogError. Please Try Again Later.");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid) 
            {
                _logger.LogError("Invalid POST attempt in {method}", nameof(CreateCountry));
                return BadRequest(ModelState); 
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);

                await _unityOfWork.Countries.Insert(country);
                await _unityOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in the {method}", nameof(CreateCountry));
                return StatusCode(500, "Ïnternal Server LogError. Please Try Again Later.");
            }
        }

        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            _logger.LogInformation("Updating country of id {id}", id);
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError("Invalid PUT attempt in {method}", nameof(UpdateCountry));
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unityOfWork.Countries.Get(q => q.Id == id);

                if(country == null)
                {
                    _logger.LogError("Invalid PUT attempt in {method} record not found", nameof(UpdateCountry));
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(countryDTO, country);
                _unityOfWork.Countries.Update(country);
                await _unityOfWork.Save();
                return NoContent();

            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Something went wrong in the {method}", nameof(UpdateCountry));
                return StatusCode(500, "Ïnternal Server LogError. Please Try Again Later.");
            }
        }

        //[Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if(id < 1)
            {
                _logger.LogError("Invalid Delete attempt in {method} with id {id}", nameof(DeleteCountry), id);
                return BadRequest("Invalid id supply");
            }

            try
            {
                var country = await _unityOfWork.Countries.Get(q => q.Id == id);

                if(country is null)
                {
                    _logger.LogError("Invalid Delete attempt in {method}. Record not found for {id}", nameof(DeleteCountry), id);
                    return BadRequest("Submitted data is invalid");
                }

                await _unityOfWork.Countries.Delete(id);
                await _unityOfWork.Save();
                return Ok("Country deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in the {method}", nameof(DeleteCountry));
                return StatusCode(500, "Ïnternal Server LogError. Please Try Again Later.");
            }
        }
    }
}
