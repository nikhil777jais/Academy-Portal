using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class RoleDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please Select Any role")]
        public string Name { get; set; }
    }
}
