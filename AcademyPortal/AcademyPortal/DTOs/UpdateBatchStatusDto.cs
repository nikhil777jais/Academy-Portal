using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class UpdateBatchStatusDto
    {
        [Required(ErrorMessage = "Please Select Any Status")]
        public string? Status { get; set; }
    }
}
