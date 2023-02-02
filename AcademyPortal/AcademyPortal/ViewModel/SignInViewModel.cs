using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class SignInViewModel
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
