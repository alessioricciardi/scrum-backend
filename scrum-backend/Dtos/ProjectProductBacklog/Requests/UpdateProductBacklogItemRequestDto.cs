using scrum_backend.Models.Projects;
using scrum_backend.Models.Sprints;

namespace scrum_backend.Dtos.ProjectProductBacklog.Requests
{
    public class UpdateProductBacklogItemRequestDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ProductBacklogItemType Type { get; set; }
        public required ProductBacklogItemStatus Status { get; set; }
        public required int Priority { get; set; }
        public int? StoryPoints { get; set; }
    }
}
