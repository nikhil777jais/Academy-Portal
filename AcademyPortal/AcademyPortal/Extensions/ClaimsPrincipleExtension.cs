using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace AcademyPortal.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUserId(this ClaimsPrincipal userClaims){
            //return  id of current user
            if (userClaims == null)
                throw new ArgumentNullException(nameof(userClaims));
            return userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string GetUsername(this ClaimsPrincipal userClaims){
            //return userName of current user
            if (userClaims == null)
                throw new ArgumentNullException(nameof(userClaims));
            return userClaims.FindFirst(ClaimTypes.Name).Value;
        }
    }
}