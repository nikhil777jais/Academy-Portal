using AcademyPortalAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyPortalAPI.Models
{
    public class Batch
    {
        public int Id { get; set; }

        public Skill RelatedSkill { get; set; }

        public Module RelatedModule { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Technology { get; set; } 

        public DateTime? Batch_Start_Date { get; set; } 

        public DateTime? Batch_End_Date { get; set; } 

        public int? Batch_Capacity { get; set; } 

        [StringLength(20, MinimumLength = 3)]
        public string? Classroom_Name { get; set; } 
        
        public ApplicationUser CreatedBy { get; set; }

        public ICollection<BatchUser>? Users { get; set; }
    }
}

