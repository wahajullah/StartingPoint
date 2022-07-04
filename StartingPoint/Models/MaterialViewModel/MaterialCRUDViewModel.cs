using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.MaterialViewModel
{
    public class  MaterialCRUDViewModel : EntityBase
    {
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Code")]
        public string MaterialId { get; set; }
        [Display(Name = "Service")]
        public Int64 ServiceId { get; set; }
        public string ServiceDisplay { get; set; }
        [Display(Name = "MaterialGroup")]
        public Int64 MaterialGroupId { get; set; }
        public string MaterialGroupDisplay { get; set; }
        public string Description { get; set; }
        [Display(Name = "Unit")]
        public string Unit { get; set; }
        public double? Rate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        [Display(Name = "Supplier")]
        public Int64 SupplierId { get; set; }
        public string SupplierDisplay { get; set; }
        public string Size { get; set; }
        public string Weight { get; set; }
        [Display(Name = "Country")]
        public Int64 CountryId { get; set; }
        public string CountryDisplay { get; set; }

        public static implicit operator MaterialCRUDViewModel(Material _Material)
        {
            return new MaterialCRUDViewModel            {
                                
                Id = _Material.Id,
                MaterialId = _Material.MaterialId,
                ServiceId = _Material.ServiceId,
                MaterialGroupId = _Material.MaterialGroupId,               
                Description = _Material.Description,    
                Unit = _Material.Unit,
                Rate = _Material.Rate,
                Brand = _Material.Brand,
                Model = _Material.Model,
                Manufacturer = _Material.Manufacturer,
                SupplierId = _Material.SupplierId,
                Size = _Material.Size,
                Weight = _Material.Weight,
                CountryId = _Material.CountryId,
                
                CreatedDate = _Material.CreatedDate,
                ModifiedDate = _Material.ModifiedDate,
                CreatedBy = _Material.CreatedBy,
                ModifiedBy = _Material.ModifiedBy,
                Cancelled = _Material.Cancelled,
            };
        }

        public static implicit operator Material(MaterialCRUDViewModel vm)
        {
            return new Material
            {                
                Id = vm.Id,
                MaterialId = vm.MaterialId,
                ServiceId = vm.ServiceId,
                MaterialGroupId = vm.MaterialGroupId,
                Description= vm.Description,
                Unit= vm.Unit,
                Rate= vm.Rate,
                Brand= vm.Brand,
                Model= vm.Model,
                Manufacturer = vm.Manufacturer,
                SupplierId = vm.SupplierId,
                Size = vm.Size,
                Weight = vm.Weight,
                CountryId= vm.CountryId,
                
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
