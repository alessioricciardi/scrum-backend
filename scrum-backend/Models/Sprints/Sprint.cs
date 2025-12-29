using scrum_backend.Models.Projects;

namespace scrum_backend.Models.Sprint
{
    public class Sprint
    {
        public int Id { get; set; }

        public required int ProjectId { get; set; }
        public required Project Project { get; set; }

        public ICollection<SprintBacklogItem> SprintBacklogItems { get; set; } = [];

        public SprintStatus Status { get; set; } = SprintStatus.InProgress;

        public DateTime StartTime {  get; private set; } = DateTime.Now;

        public required DateTime EndTime { get; set;}
    }
    public enum SprintStatus
    {
        InProgress = 0,
        Archival = 1,
    }
}
