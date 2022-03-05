using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models
{
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength:50, ErrorMessage ="Country name is too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength:2, ErrorMessage ="Expected Country short name is 2 character")]
        public string ShortName { get; set; }
    }
    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }

        public virtual ICollection<HotelDTO> Hotels { get; set; }
    }
}
