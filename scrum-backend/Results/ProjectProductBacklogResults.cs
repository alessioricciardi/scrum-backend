using scrum_backend.Dtos.ProjectProductBacklog.Responses;

namespace scrum_backend.Results
{
    public record ProductBacklogItemNotFound;

    public record GetProductBacklogItemsSucceeded(ICollection<GetProductBacklogItemResponseDto> GetProductBacklogItemsDto);
    public record GetProductBacklogItemByIdSucceeded(GetProductBacklogItemResponseDto GetProductBacklogItemDto);

    public record CreateProductBacklogItemSucceeded(CreateProductBacklogItemResponseDto CreateProductBacklogItemResponseDto);
    public record CreateProductBacklogItemFailed;

    public record UpdateProductBacklogItemSucceeded(UpdateProductBacklogItemResponseDto UpdateProductBacklogItemResponseDto);
    public record UpdateProductBacklogItemFailed;

    public record DeleteProductBacklogItemSucceeded;
    public record DeleteProductBacklogItemFailed;
}
