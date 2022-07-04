using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.MaterialGroupViewModel
{
    public class  MaterialGroupCRUDViewModel : EntityBase
    {
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Code")]
        public string MaterialGroupId { get; set; }
        [Display(Name = "Service")]
        public Int64 ServiceId { get; set; }
        public string ServiceDisplay { get; set; }

        public string Description { get; set; }

        public static implicit operator MaterialGroupCRUDViewModel(MaterialGroup _MaterialGroup)
        {
            return new MaterialGroupCRUDViewModel
            {
                
                Id = _MaterialGroup.Id,
                MaterialGroupId = _MaterialGroup.MaterialGroupId,
                ServiceId = _MaterialGroup.ServiceId,
                Description = _MaterialGroup.Description,               
                
                CreatedDate = _MaterialGroup.CreatedDate,
                ModifiedDate = _MaterialGroup.ModifiedDate,
                CreatedBy = _MaterialGroup.CreatedBy,
                ModifiedBy = _MaterialGroup.ModifiedBy,
                Cancelled = _MaterialGroup.Cancelled,
            };
        }

        public static implicit operator MaterialGroup(MaterialGroupCRUDViewModel vm)
        {
            return new MaterialGroup
            {
                Id = vm.Id,
                MaterialGroupId = vm.MaterialGroupId,
                ServiceId = vm.ServiceId,
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
