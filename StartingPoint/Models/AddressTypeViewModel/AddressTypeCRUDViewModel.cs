using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.AddressTypeViewModel
{
    public class AddressTypeCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string AddressTypeId { get; set; }
        public string Description { get; set; }



        public static implicit operator AddressTypeCRUDViewModel(AddressType _AddressType)
        {
            return new AddressTypeCRUDViewModel
            {
                Id = _AddressType.Id,
                AddressTypeId = _AddressType.AddressTypeId,
                Description = _AddressType.Description,
                CreatedDate = _AddressType.CreatedDate,
                ModifiedDate = _AddressType.ModifiedDate,
                CreatedBy = _AddressType.CreatedBy,
                ModifiedBy = _AddressType.ModifiedBy,
                Cancelled = _AddressType.Cancelled,
            };
        }

        public static implicit operator AddressType(AddressTypeCRUDViewModel vm)
        {
            return new AddressType
            {
                Id = vm.Id,
                AddressTypeId = vm.AddressTypeId,
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
