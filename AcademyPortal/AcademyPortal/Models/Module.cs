using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyPortal.Models
{
    public class Module
    {
        public int Id { get; set; }
        
        [StringLength(20, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Technology { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Proficiency { get; set; }

        [ForeignKey("ModuleId")]
        public ICollection<Skill>? RelatedSkills { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("ModuleId")]
        public ICollection<Batch>? RelatedBatches { get; set; }
    }
}
