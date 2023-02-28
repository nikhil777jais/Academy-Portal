using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class SkillModuleMappingDto
    {
        [Required(ErrorMessage = "Please Select Any Module")]
        public List<string> ModuleNames { get; set; }
    }
}
