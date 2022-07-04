using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.DepartmentViewModel
{
    public class DepartmentCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string DepartmentId { get; set; }
        public string Description { get; set; }



        public static implicit operator DepartmentCRUDViewModel(Department _Department)
        {
            return new DepartmentCRUDViewModel
            {
                Id = _Department.Id,
                DepartmentId = _Department.DepartmentId,
                Description = _Department.Description,
                CreatedDate = _Department.CreatedDate,
                ModifiedDate = _Department.ModifiedDate,
                CreatedBy = _Department.CreatedBy,
                ModifiedBy = _Department.ModifiedBy,
                Cancelled = _Department.Cancelled,
            };
        }

        public static implicit operator Department(DepartmentCRUDViewModel vm)
        {
            return new Department
            {
                Id = vm.Id,
                DepartmentId = vm.DepartmentId,
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
