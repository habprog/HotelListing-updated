﻿using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
               new Hotel
               {
                   Id = 1,
                   Name = "Sandal Resort and Spa",
                   Address = "Negril",
                   CountryId = 1,
                   Rating = 4.5
               },
               new Hotel
               {
                   Id = 2,
                   Name = "Confort Suite",
                   Address = "George Town",
                   CountryId = 2,
                   Rating = 4.3
               },
               new Hotel
               {
                   Id = 3,
                   Name = "Grand Palldium",
                   Address = "Nassua",
                   CountryId = 3,
                   Rating = 4
               }
           );
        }
    }
}
