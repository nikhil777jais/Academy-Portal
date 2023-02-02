using AcademyPortal.Model;

namespace AcademyPortal.Models
{
    public class BatchUser
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int BatchId { get; set; }
        public Batch Batch { get; set; }
        
       public Status? status { get; set; } 
    }
}
