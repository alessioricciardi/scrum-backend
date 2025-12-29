using scrum_backend.Models.Projects;

namespace scrum_backend.Models.Sprint
{
    public class SprintBacklogItem
    {
        public int Id { get; set; }
        public required int SprintId { get; set; }
        public required Sprint Sprint { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public SprintBacklogItemStatus Status { get; set; } = SprintBacklogItemStatus.ToDo;

        public ICollection<ProjectMember> MembersAssigned { get; set; } = [];
    }
    public enum SprintBacklogItemStatus
    {
        ToDo = 0,
        InProgress = 1,
        InOverview = 2,
        Done = 3,
    }
}
