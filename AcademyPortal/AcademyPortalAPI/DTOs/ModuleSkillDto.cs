using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.DTOs
{
    public class ModuleSkillDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Family { get; set; }

        public string? CreatedBy { get; set; }

        public List<ModuleDto> RelatedModules { get; set; }
    }
}
