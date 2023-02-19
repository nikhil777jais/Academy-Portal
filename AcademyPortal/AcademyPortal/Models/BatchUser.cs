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
// To avoid redundancy of batch data, Created this bridge table
// this table table only holds specific data