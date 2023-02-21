using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class UpdateStatusDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Select Any Status")]
        public string Status { get; set; }
    }
}
