using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class RoleViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Role")]
        public string? Name { get; set; }
    }
}
