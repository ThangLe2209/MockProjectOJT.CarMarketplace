namespace AuthenticationApi.Application.DTOs
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Value { get; set; }
        public int UserId { get; set; }
    }
}
