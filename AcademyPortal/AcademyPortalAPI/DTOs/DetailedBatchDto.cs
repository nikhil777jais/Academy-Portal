namespace AcademyPortalAPI.DTOs
{
    public class DetailedBatchDto
    {
        public int? Id { get; set; }
        public string? Technology { get; set; }
        public DateTime? Batch_Start_Date { get; set; }
        public DateTime? Batch_End_Date { get; set; }
        public int? Batch_Capacity { get; set; }
        public string? Classroom_Name { get; set; }
        public ModuleNameDto RelatedModule { get; set; }
        public SkillNameDto RelatedSkill { get; set; }
        public string CreatedBy { get; set; }
        public List<BatchUserStatusDto>? Users { get; set; }
    }
}
