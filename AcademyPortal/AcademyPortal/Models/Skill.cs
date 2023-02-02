using AcademyPortal.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyPortal.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Family { get; set; }

        [ForeignKey("SkillId")]
        public ICollection<Module>? RelatedModules { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        [ForeignKey("SkillId")]
        public ICollection<Batch>? RelatedBatches { get; set; }

    }
}
