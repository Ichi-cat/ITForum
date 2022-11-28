using MediatR;

namespace ITForum.Application.Users.Commands.UpdateUserInfo
{
    public class UpdateUserInfoCommand:IRequest
    {
        public Guid UserId { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? Location { get; set; }
        public string? Study { get; set; }
        public string? Work { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
