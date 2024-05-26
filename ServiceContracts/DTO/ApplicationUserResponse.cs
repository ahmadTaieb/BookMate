
using BookMate.Entities;

namespace ServiceContracts.DTO
{
    public class ApplicationUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisteredAt { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
