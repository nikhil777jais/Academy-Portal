using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyPortal.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<ApplicationUser>? users { get; set; }

        [ForeignKey("StatusId")]
        public ICollection<BatchUser>? Batches { get; set; }


    }
}
