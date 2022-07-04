using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.ServiceViewModel
{
    public class ServiceCRUDViewModel : EntityBase
    {
        [Key]      
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Code")]
        public string ServiceId { get; set; }
        [Required]
        [StringLength(2),MinLength(2)]
        [Display(Name = "Short Text")]       
        public string ShortText { get; set; }
        public string Description { get; set; }



        public static implicit operator ServiceCRUDViewModel(Service _Service)
        {
            return new ServiceCRUDViewModel
            {
                Id = _Service.Id,
                ServiceId = _Service.ServiceId,
                ShortText = _Service.ShortText,
                Description = _Service.Description,
                CreatedDate = _Service.CreatedDate,
                ModifiedDate = _Service.ModifiedDate,
                CreatedBy = _Service.CreatedBy,
                ModifiedBy = _Service.ModifiedBy,
                Cancelled = _Service.Cancelled,
            };
        }

        public static implicit operator Service(ServiceCRUDViewModel vm)
        {
            return new Service
            {
                Id = vm.Id,
                ServiceId = vm.ServiceId,
                ShortText = vm.ShortText,
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
