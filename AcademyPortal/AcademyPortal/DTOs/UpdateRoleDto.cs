using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Please Select Any role")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Please Select Any Status")]
        public string? Status { get; set; }
    }
}
