using scrum_backend.Models.AppUsers;

namespace scrum_backend.Models.Projects
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required string OwnerId { get; set; }
        public AppUser? Owner { get; set; }

        public ICollection<ProjectMember> Members { get; set; } = [];
        public ICollection<ProductBacklogItem> ProductBacklogItems { get; set; } = [];

        public DateOnly DateCreated { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        
    }
}
