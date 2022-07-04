using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.CityViewModel
{
    public class CityCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string CityID { get; set; }
        public string Name { get; set; }



        public static implicit operator CityCRUDViewModel(City _City)
        {
            return new CityCRUDViewModel
            {
                Id = _City.Id,
                CityID = _City.CityID,
                Name = _City.Name,
                CreatedDate = _City.CreatedDate,
                ModifiedDate = _City.ModifiedDate,
                CreatedBy = _City.CreatedBy,
                ModifiedBy = _City.ModifiedBy,
                Cancelled = _City.Cancelled,
            };
        }

        public static implicit operator City(CityCRUDViewModel vm)
        {
            return new City
            {
                Id = vm.Id,
                CityID = vm.CityID,
                Name = vm.Name,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
