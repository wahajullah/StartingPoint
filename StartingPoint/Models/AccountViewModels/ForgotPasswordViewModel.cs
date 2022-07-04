using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
