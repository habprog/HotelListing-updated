using HotelListing.API.Models;
using System.Threading.Tasks;

namespace HotelListing.API.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
