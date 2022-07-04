using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.AddressBookViewModel
{
    public class  AddressBookCRUDViewModel : EntityBase
    {
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Code")]
        public string AddressId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Direct Phone")]
        public string PPhone { get; set; }
        public string Mobile { get; set; }

        [Display(Name = "Personal Email")]
        public string PEmail { get; set; }
        public string Company { get; set; }
        [Display(Name = "Office Email")]
        public string OEmail { get; set; }
        [Display(Name = "Office Phone")]
        public string OPhone { get; set; }
        [Display(Name = "Office Fax")]
        public string OFax { get; set; }
        public string Website { get; set; }
        [Display(Name = "P.O.Box")]
        public string Pobox { get; set; }

        [Display(Name = "Address Type")]
        public Int64 AddressTypeId { get; set; }
        public string AddressTypeDisplay { get; set; }

        [Display(Name = "City")]
        public Int64 CityId { get; set; }
        public string CityDisplay { get; set; }

        [Display(Name = "Country")]
        public Int64 CountryId { get; set; }
        public string CountryDisplay { get; set; }
        [Display(Name = "Status")]
        public Int64 StatusId { get; set; }
        public string StatusDisplay { get; set; }
        public string Address { get; set; }
              

        public static implicit operator AddressBookCRUDViewModel(AddressBook _AddressBook)
        {
            return new AddressBookCRUDViewModel
            {
                
                Id = _AddressBook.Id,
                AddressId = _AddressBook.AddressId,
                Name = _AddressBook.Name,
                JobTitle = _AddressBook.JobTitle,
                PPhone = _AddressBook.PPhone,
                Mobile = _AddressBook.Mobile,
                PEmail = _AddressBook.PEmail,
                Company =_AddressBook.Company,
                OEmail = _AddressBook.OEmail,
                OPhone = _AddressBook.OPhone,
                OFax = _AddressBook.OFax,
                Website = _AddressBook.Website,
                Pobox = _AddressBook.Pobox,
                AddressTypeId = _AddressBook.AddressTypeId,
                CityId = _AddressBook.CityId,
                CountryId = _AddressBook.CountryId,
                StatusId = _AddressBook.StatusId,
                Address = _AddressBook.Address,
                CreatedDate = _AddressBook.CreatedDate,
                ModifiedDate = _AddressBook.ModifiedDate,
                CreatedBy = _AddressBook.CreatedBy,
                ModifiedBy = _AddressBook.ModifiedBy,
                Cancelled = _AddressBook.Cancelled,
            };
        }

        public static implicit operator AddressBook(AddressBookCRUDViewModel vm)
        {
            return new AddressBook
            {
                Id = vm.Id,
                AddressId = vm.AddressId,
                Name = vm.Name,
                JobTitle = vm.JobTitle,
                PPhone = vm.PPhone,
                Mobile = vm.Mobile,
                PEmail = vm.PEmail,
                Company = vm.Company,
                OEmail = vm.OEmail,
                OPhone = vm.OPhone,
                OFax = vm.OFax,
                Website = vm.Website,
                Pobox = vm.Pobox,
                AddressTypeId = vm.AddressTypeId,
                CityId = vm.CityId,
                CountryId = vm.CountryId,
                StatusId = vm.StatusId,
                Address = vm.Address,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
