using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.UserAccountViewModel
{
    public class UserProfileViewModel : EntityBase
    {
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

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Profile Picture")]
        public string ProfilePictureURL { get; set; } = "/upload/blank-person.png";
        public IFormFile ProfilePicture { get; set; }
        public string CurrentURL { get; set; }


        public static implicit operator UserProfileViewModel(UserProfile _UserProfile)
        {
            return new UserProfileViewModel
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

                PasswordHash = _UserProfile.PasswordHash,
                ConfirmPassword = _UserProfile.ConfirmPassword,
                OldPassword = _UserProfile.OldPassword,
                CreatedDate = _UserProfile.CreatedDate,
                ModifiedDate = _UserProfile.ModifiedDate,
                CreatedBy = _UserProfile.CreatedBy,
                ModifiedBy = _UserProfile.ModifiedBy
                //ProfilePicture = _UserProfile.ProfilePicture
            };
        }

        public static implicit operator UserProfile(UserProfileViewModel vm)
        {
            return new UserProfile
            {
                UserProfileId = vm.UserProfileId,
                ApplicationUserId = vm.ApplicationUserId,
                HRNumber = vm.HRNumber,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DesignationId = vm.DesignationId,
                DepartmentId = vm.DepartmentId,
                DivisionId = vm.DivisionId,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,

                PasswordHash = vm.PasswordHash,
                ConfirmPassword = vm.ConfirmPassword,
                OldPassword = vm.OldPassword,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy
                //ProfilePicture = vm.ProfilePicture
            };
        }
    }
}
