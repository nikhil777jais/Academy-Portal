using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class UpdateRole1Dto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Select Any role")]
        public string Role { get; set; }
    }
}
