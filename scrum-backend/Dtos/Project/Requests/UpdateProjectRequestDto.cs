namespace scrum_backend.Dtos.Project.Requests
{
    public class UpdateProjectRequestDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
