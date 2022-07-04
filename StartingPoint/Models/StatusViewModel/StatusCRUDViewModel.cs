using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.StatusViewModel
{
    public class StatusCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string StatusId { get; set; }
        public string Description { get; set; }



        public static implicit operator StatusCRUDViewModel(Status Status)
        {
            return new StatusCRUDViewModel
            {
                Id = Status.Id,
                StatusId = Status.StatusId,
                Description = Status.Description,
                CreatedDate = Status.CreatedDate,
                ModifiedDate = Status.ModifiedDate,
                CreatedBy = Status.CreatedBy,
                ModifiedBy = Status.ModifiedBy,
                Cancelled = Status.Cancelled,
            };
        }

        public static implicit operator Status(StatusCRUDViewModel vm)
        {
            return new Status
            {
                Id = vm.Id,
                StatusId = vm.StatusId,
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
