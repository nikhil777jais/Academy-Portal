using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortalAPI.Models
{
    public class UserRole:IdentityUserRole<string>
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}