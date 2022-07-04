using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.UserAccountViewModel
{
    public class UserProfileCRUDViewModel : EntityBase
    {
        [Display(Name = "User Profile Id")]
        public Int64 UserProfileId { get; set; }

        public string ApplicationUserId { get; set; }
        [Display(Name = "HR #")]
        [Required]
        public string HRNumber { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Designation")]
        public Int64 DesignationId { get; set; }
        public string DesignationDisplay { get; set; }
        [Display(Name = "Department")]
        public Int64 DepartmentId { get; set; }
        public string DepartmentDisplay { get; set; }
        [Display(Name = "Division")]
        public Int64 DivisionId { get; set; }
        public string DivisionDisplay { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }
        public string ProfilePicturePath { get; set; }


        public static implicit operator UserProfileCRUDViewModel(UserProfile _UserProfile)
        {
            return new UserProfileCRUDViewModel
            {
                UserProfileId = _UserProfile.UserProfileId,
                ApplicationUserId = _UserProfile.ApplicationUserId,
                HRNumber = _UserProfile.HRNumber,
                FirstName = _UserProfile.FirstName,
                LastName = _UserProfile.LastName,
                DesignationId = _UserProfile.DesignationId, 
                DepartmentId = _UserProfile.DepartmentId,
                DivisionId = _UserProfile.DivisionId, 
                PhoneNumber = _UserProfile.PhoneNumber,
                Email = _UserProfile.Email,
                Address = _UserProfile.Address,
                Country = _UserProfile.Country,

                CreatedDate = _UserProfile.CreatedDate,
                ModifiedDate = _UserProfile.ModifiedDate,
                CreatedBy = _UserProfile.CreatedBy,
                ModifiedBy = _UserProfile.ModifiedBy,
                ProfilePicturePath = _UserProfile.ProfilePicture
            };
        }

        public static implicit operator UserProfile(UserProfileCRUDViewModel vm)
        {
            return new UserProfile
            {
                UserProfileId = vm.UserProfileId,
                ApplicationUserId = vm.ApplicationUserId,
                HRNumber=vm.HRNumber,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DesignationId = vm.DesignationId,
                DepartmentId = vm.DepartmentId,
                DivisionId = vm.DivisionId,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                ProfilePicture = vm.ProfilePicturePath
            };
        }
    }
}
