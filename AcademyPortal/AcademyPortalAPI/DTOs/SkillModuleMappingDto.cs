using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class SkillModuleMappingDto
    {

        [Required(ErrorMessage = "Please provide module id")]
        public List<int> ModuleIds { get; set; }
    }
}
