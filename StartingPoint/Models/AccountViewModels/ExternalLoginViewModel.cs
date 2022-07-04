using System.ComponentModel.DataAnnotations;

namespace StartingPoint.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
