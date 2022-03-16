using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models;

namespace HotelListing.API.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();

            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
