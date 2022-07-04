using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.PaymentTermViewModel
{
    public class PaymentTermCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string PaymentTermId { get; set; }
        public string Description { get; set; }



        public static implicit operator PaymentTermCRUDViewModel(PaymentTerm _PaymentTerm)
        {
            return new PaymentTermCRUDViewModel
            {
                Id = _PaymentTerm.Id,
                PaymentTermId = _PaymentTerm.PaymentTermId,
                Description = _PaymentTerm.Description,
                CreatedDate = _PaymentTerm.CreatedDate,
                ModifiedDate = _PaymentTerm.ModifiedDate,
                CreatedBy = _PaymentTerm.CreatedBy,
                ModifiedBy = _PaymentTerm.ModifiedBy,
                Cancelled = _PaymentTerm.Cancelled,
            };
        }

        public static implicit operator PaymentTerm(PaymentTermCRUDViewModel vm)
        {
            return new PaymentTerm
            {
                Id = vm.Id,
                PaymentTermId = vm.PaymentTermId,
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
