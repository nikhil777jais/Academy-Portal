using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortalAPI.Models
{
    public class ApplicationRole:IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}