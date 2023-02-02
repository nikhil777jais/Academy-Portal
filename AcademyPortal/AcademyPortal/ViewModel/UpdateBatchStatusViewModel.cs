using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class UpdateBatchStatusViewModel
    {
        [Required(ErrorMessage = "Please Select Any Status")]
        public string? Status { get; set; }
    }
}
