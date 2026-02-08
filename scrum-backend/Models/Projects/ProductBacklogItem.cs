using scrum_backend.Models.Sprints;

namespace scrum_backend.Models.Projects
{
    public class ProductBacklogItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ProductBacklogItemType Type { get; set; }
        public ProductBacklogItemStatus Status { get; set; } = ProductBacklogItemStatus.ProductBacklog;

        public int Priority { get; set; } = 0;
        public int? StoryPoints { get; set; }
        
        public required int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int? SprintId { get; set; }
        public Sprint? Sprint { get; set; }

        public DateOnly DateCreated { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
