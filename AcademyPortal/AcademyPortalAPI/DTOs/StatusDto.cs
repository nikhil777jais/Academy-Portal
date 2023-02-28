using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class StatusDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Select Any Status")]
        public string Name { get; set; }
    }
}
