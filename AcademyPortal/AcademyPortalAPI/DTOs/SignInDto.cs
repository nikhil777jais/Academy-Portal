using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
