using System;

namespace StartingPoint.Models
{
    public class UserProfile : EntityBase
    {
        public Int64 UserProfileId { get; set; }
        public string ApplicationUserId { get; set; }
        public string HRNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int64 DesignationId { get; set; }
        public Int64 DepartmentId { get; set; }
        public Int64 DivisionId { get; set; }       
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        
    }
}
