namespace AcademyPortal.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? ExpiresIn { get; set; }
    }
}
    