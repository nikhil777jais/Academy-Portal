using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class SkillDto
    {
        [Required(ErrorMessage = "Please Enter Skill Name")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Skill Family")]
        [StringLength(20, MinimumLength = 3)]
        public string Family { get; set; }
    }
}
