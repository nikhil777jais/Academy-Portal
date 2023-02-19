using AcademyPortal.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyPortal.Models
{
    public class ApplicationUser:IdentityUser
    {
        [StringLength(20, MinimumLength = 3)]
        public string? FirstName { get; set; }
    
        [StringLength(20, MinimumLength = 3)]
        public string? LastName { get; set; } 

        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(20)]
        public string? Gender { get; set; }

        public DateTime? DateJoined { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; } = DateTime.Now;

        public Status? status { get; set; }

        [ForeignKey("createdBy")]
        public ICollection<Skill>? CreatedSkills { get; set; }
        
        [ForeignKey("createdBy")]
        public ICollection<Module>? CreatedModules { get; set; }

        [ForeignKey("createdBy")]
        public ICollection<Batch>? CreatedBatches { get; set; }        

        public ICollection<BatchUser>? Batches { get; set; }
        
    }
}
