using scrum_backend.Models.Projects;

namespace scrum_backend.Dtos.ProjectProductBacklog.Responses
{
    public class UpdateProductBacklogItemResponseDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ProductBacklogItemType Type { get; set; }
        public required ProductBacklogItemStatus Status { get; set; }
        public required int Priority { get; set; }
        public int? StoryPoints { get; set; }
    }
}
