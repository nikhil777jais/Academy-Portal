using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class SignUpUserViewModel
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress]
        [Display(Name ="Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Confirm password is not matched")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
