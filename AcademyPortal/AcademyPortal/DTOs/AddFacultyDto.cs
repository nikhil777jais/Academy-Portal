using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.DTOs
{
    public class AddFacultyDto
    {
        [Required(ErrorMessage = "Please Select Faculty")]
        public List<string> Faculties { get; set; }
    }
}
