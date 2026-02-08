using scrum_backend.Models.Projects;

namespace scrum_backend.Models.Sprints
{
    public class Sprint
    {
        private const int DefaultSprintDurationInDays = 14;

        public int Id { get; set; }

        public required int ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<SprintBacklogItem> SprintBacklogItems { get; set; } = [];

        public SprintStatus Status { get; set; } = SprintStatus.InProgress;

        public DateTime StartTime {  get; private set; } = DateTime.Now;

        public DateTime EndTime { get; set; } = DateTime.Now.AddDays(DefaultSprintDurationInDays);
    }
}
