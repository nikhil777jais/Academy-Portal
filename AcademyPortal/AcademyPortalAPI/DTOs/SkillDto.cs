using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class SkillDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Skill Name")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Skill Family")]
        [StringLength(20, MinimumLength = 3)]
        public string Family { get; set; }

        public string? CreatedBy { get; set; }
    }
}
