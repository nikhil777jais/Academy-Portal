using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class UpdateBatchStatusDto
    {
        [Required(ErrorMessage = "Please Select Any Status")]
        public int StatusId { get; set; }
    }
}
