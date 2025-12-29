using scrum_backend.Models.AppUsers;

namespace scrum_backend.Models.Projects
{
    public class ProjectMember
    {
        public int Id { get; set; }

        public required int ProjectId { get; set; }
        public Project? Project { get; set; }

        public required string UserId { get; set; }
        public AppUser? User {  get; set; }

        public required ProjectMemberRole Role { get; set; }
    }
}
