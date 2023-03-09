using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class BatchDto
    {
        public int? Id { get; set; }

        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Please Select Any Skill")]
        public int RelatedSkillId { get; set; }

        [Required(ErrorMessage = "Please Select Any Module")]
        [Display(Name = "Module")]
        public int RelatedModuleId { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string? Technology { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Batch Start Date")]
        public DateTime? Batch_Start_Date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Batch End Date")]
        public DateTime? Batch_End_Date { get; set; }

        [Required]
        [Display(Name = "Batch Capacity")]
        public int? Batch_Capacity { get; set; }

        [Required]
        [Display(Name = "Classroom Name ")]
        [StringLength(20, MinimumLength = 3)]
        public string? Classroom_Name { get; set; }

        public string? CreatedBy { get; set; }
    }
}
