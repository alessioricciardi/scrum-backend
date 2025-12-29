using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using scrum_backend.Models.AppUsers;
using scrum_backend.Models.Projects;
using scrum_backend.Models.Sprint;

namespace scrum_backend.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProductBacklogItem> ProductBacklogItems { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<SprintBacklogItem> SprintBacklogItems { get; set; }
    }
}
