﻿using System.ComponentModel.DataAnnotations;

namespace AcademyPortalAPI.DTOs
{
    public class ModuleDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Module")]
        [StringLength(20, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Technology of Module ")]
        [StringLength(20, MinimumLength = 3)]
        public string? Technology { get; set; }

        [Required(ErrorMessage = "Please Enter Proficiency in Module")]
        [StringLength(20, MinimumLength = 3)]
        public string? Proficiency { get; set; }

        public string? CreatedBy { get; set; }
    }
}
