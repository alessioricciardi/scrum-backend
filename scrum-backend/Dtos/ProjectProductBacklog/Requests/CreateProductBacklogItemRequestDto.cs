using scrum_backend.Models.Projects;
using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.ProjectProductBacklog.Requests
{
    public class CreateProductBacklogItemRequestDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [EnumDataType(typeof(ProductBacklogItemType))]
        public ProductBacklogItemType Type { get; set; }

        [Range(0, int.MaxValue)]
        public int Priority { get; set; } = 0;
        public int? StoryPoints { get; set; }
    }
}
