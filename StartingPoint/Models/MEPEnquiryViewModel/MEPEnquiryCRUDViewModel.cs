using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.MEPEnquiryViewModel
{
    public class  MEPEnquriyCRUDViewModel : EntityBase
    {
        [Required]
        public Int64 Id { get; set; }
        [Display(Name ="Code")]
        public string MEPEnquiryId { get; set; }
        public DateTime Date { get; set; }
        [Display(Name="Service")]
        public Int64 ServiceId { get; set; }
        public string ServiceDisplay { get; set; }
        public string Project { get; set; }
        [Display(Name = "MainContractor")]
        public Int64 MContractorId { get; set; }
        public string MContractorDisplay { get; set; }
        [Display(Name = "Consultant")]
        public Int64 ConsultantId { get; set; }
        public string ConsultantDisplay { get; set; }
        [Display(Name = "Client")]
        public Int64 ClientId { get; set; }
        public string ClientDisplay { get; set; }
        public DateTime DueDate { get; set; }
        public double? TenderPrice { get; set; }
        [Display(Name = "Status")]
        public Int64 StatusId { get; set; }
        public string StatusDisplay { get; set; }
        [Display(Name = "City")]
        public Int64 CityId { get; set; }
        public string CityDisplay { get; set; }
        [Display(Name = "Country")]
        public Int64 CountryId { get; set; }
        public string CountryDisplay { get; set; }
        public String Notes { get; set; }


        
              

        public static implicit operator MEPEnquriyCRUDViewModel(MEPEnquiry _MEPEnquiry)
        {
            return new MEPEnquriyCRUDViewModel
            {
               
                Id = _MEPEnquiry.Id,
                MEPEnquiryId = _MEPEnquiry.MEPEnquiryId,
                Date = _MEPEnquiry.Date,
                ServiceId = _MEPEnquiry.ServiceId,
                Project = _MEPEnquiry.Project,
                MContractorId = _MEPEnquiry.MContractorId,
                ConsultantId = _MEPEnquiry.ConsultantId,
                ClientId = _MEPEnquiry.ClientId,
                DueDate = _MEPEnquiry.DueDate,
                TenderPrice = _MEPEnquiry.TenderPrice,
                StatusId = _MEPEnquiry.StatusId,
                CityId = _MEPEnquiry.CityId,
                CountryId = _MEPEnquiry.CountryId,
                Notes = _MEPEnquiry.Notes,                
                CreatedDate = _MEPEnquiry.CreatedDate,
                ModifiedDate = _MEPEnquiry.ModifiedDate,
                CreatedBy = _MEPEnquiry.CreatedBy,
                ModifiedBy = _MEPEnquiry.ModifiedBy,
                Cancelled = _MEPEnquiry.Cancelled,
            };
        }

        public static implicit operator MEPEnquiry(MEPEnquriyCRUDViewModel vm)
        {
            return new MEPEnquiry
            {
                Id = vm.Id,
                MEPEnquiryId = vm.MEPEnquiryId,
                Date = vm.Date,
                ServiceId = vm.ServiceId,
                Project = vm.Project,
                MContractorId = vm.MContractorId,
                ConsultantId = vm.ConsultantId,
                ClientId = vm.ClientId,
                DueDate = vm.DueDate,
                TenderPrice = vm.TenderPrice,
                StatusId = vm.StatusId,
                CityId = vm.CityId,
                CountryId = vm.CountryId,              
                Notes = vm.Notes,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
