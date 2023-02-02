using System.ComponentModel.DataAnnotations;

namespace AcademyPortal.ViewModel
{
    public class AddFacultyViewModel
    {
        [Required(ErrorMessage = "Please Select Faculty")]
        public List<string> Faculties { get; set; }
    }
}
