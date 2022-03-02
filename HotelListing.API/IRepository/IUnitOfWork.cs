using HotelListing.API.Data;
using System;
using System.Threading.Tasks;

namespace HotelListing.API.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
