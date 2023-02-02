using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class SkillModuleMappingViewModel
    {
        [Required(ErrorMessage = "Please Select Any Module")]
        public List<string> ModuleNames { get; set; }
    }
}
