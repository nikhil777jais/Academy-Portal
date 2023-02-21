using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class ProfileDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        public string? Status { get; set; }

        public string? Role { get; set; }

    }
}
