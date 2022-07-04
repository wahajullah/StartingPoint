using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.CountryViewModel
{
    public class CountryCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string CountryId { get; set; }
        public string Description { get; set; }



        public static implicit operator CountryCRUDViewModel(Country _Country)
        {
            return new CountryCRUDViewModel
            {
                Id = _Country.Id,
                CountryId = _Country.CountryId,
                Description = _Country.Description,
                CreatedDate = _Country.CreatedDate,
                ModifiedDate = _Country.ModifiedDate,
                CreatedBy = _Country.CreatedBy,
                ModifiedBy = _Country.ModifiedBy,
                Cancelled = _Country.Cancelled,
            };
        }

        public static implicit operator Country(CountryCRUDViewModel vm)
        {
            return new Country
            {
                Id = vm.Id,
                CountryId = vm.CountryId,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
