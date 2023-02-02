using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class ModuleViewModel
    {
        [Required(ErrorMessage = "Please Enter Name of Module")]
        [StringLength(20, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Technology of Module ")]
        [StringLength(20, MinimumLength = 3)]
        public string? Technology { get; set; }

        [Required(ErrorMessage = "Please Enter Proficiency in Module")]
        [StringLength(20, MinimumLength = 3)]
        public string? Proficiency { get; set; }
    }
}
