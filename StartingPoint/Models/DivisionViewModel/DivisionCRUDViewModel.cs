using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.DivisionViewModel
{
    public class DivisionCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string DivisionId { get; set; }
        public string Description { get; set; }



        public static implicit operator DivisionCRUDViewModel(Division _Division)
        {
            return new DivisionCRUDViewModel
            {
                Id = _Division.Id,
                DivisionId = _Division.DivisionId,
                Description = _Division.Description,
                CreatedDate = _Division.CreatedDate,
                ModifiedDate = _Division.ModifiedDate,
                CreatedBy = _Division.CreatedBy,
                ModifiedBy = _Division.ModifiedBy,
                Cancelled = _Division.Cancelled,
            };
        }

        public static implicit operator Division(DivisionCRUDViewModel vm)
        {
            return new Division
            {
                Id = vm.Id,
                DivisionId = vm.DivisionId,
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
