namespace scrum_backend.Dtos
{
    public class ErrorResponseDto
    {
        public required string Message { get; set; }

        public IEnumerable<String>? Errors { get; set; }
    }
}
