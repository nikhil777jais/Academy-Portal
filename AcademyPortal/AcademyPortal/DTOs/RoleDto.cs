using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class RoleDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Role")]
        public string? Name { get; set; }
    }
}
