using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.DesignationViewModel
{
    public class DesignationCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string DesignationId { get; set; }
        public string Description { get; set; }



        public static implicit operator DesignationCRUDViewModel(Designation _Designation)
        {
            return new DesignationCRUDViewModel
            {
                Id = _Designation.Id,
                DesignationId = _Designation.DesignationId,
                Description = _Designation.Description,
                CreatedDate = _Designation.CreatedDate,
                ModifiedDate = _Designation.ModifiedDate,
                CreatedBy = _Designation.CreatedBy,
                ModifiedBy = _Designation.ModifiedBy,
                Cancelled = _Designation.Cancelled,
            };
        }

        public static implicit operator Designation(DesignationCRUDViewModel vm)
        {
            return new Designation
            {
                Id = vm.Id,
                DesignationId = vm.DesignationId,
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
