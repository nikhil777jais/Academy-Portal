using AcademyPortal.Models;
using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class AddBatchViewModel
    {
        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Please Select Any Skill")]
        public string RelaedSkill { get; set; }

        [Required(ErrorMessage = "Please Select Any Module")]
        [Display(Name = "Module")]
        public string RelaedModule { get; set; }

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
    }
}
