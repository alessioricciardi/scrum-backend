namespace scrum_backend.Models.Projects
{
    public class ProductBacklogItem
    {
        public int Id { get; set; }
        public required int ProjectId { get; set; }
        public required Project Project { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required ProductBacklogItemType Type { get; set; }

        public int Priority { get; set; } = 0;

        public int? StoryPoints { get; set; }

        public DateOnly DateCreated { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

    }
    public enum ProductBacklogItemType
    {
        UserStory = 0,
        Epic = 1,
        NFR = 2,
        Spike = 3
    }
}
