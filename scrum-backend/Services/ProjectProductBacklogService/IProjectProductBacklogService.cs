using OneOf;
using scrum_backend.Dtos.ProjectProductBacklog.Requests;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectProductBacklogService
{
    public interface IProjectProductBacklogService
    {
        Task<OneOf<GetProductBacklogItemsSucceeded, ProjectNotFound, Forbidden>> GetProductBacklogItemsAsync(int projectId, ClaimsPrincipal user);

        Task <OneOf<GetProductBacklogItemByIdSucceeded, ProjectNotFound, ProductBacklogItemNotFound, Forbidden>> GetProductBacklogItemByIdAsync(int projectId, int productBacklogItemId, ClaimsPrincipal user);
         
        Task <OneOf<CreateProductBacklogItemSucceeded, CreateProductBacklogItemFailed, ProjectNotFound, InvalidType, Forbidden>> CreateProductBacklogItemAsync(int projectId, CreateProductBacklogItemRequestDto createItemDto, ClaimsPrincipal user);

        Task <OneOf<UpdateProductBacklogItemSucceeded, UpdateProductBacklogItemFailed, ProjectNotFound, ProductBacklogItemNotFound, InvalidType, Forbidden>> UpdateProductBacklogItemAsync(int projectId, int productBacklogItemId, UpdateProductBacklogItemRequestDto updateItemDto, ClaimsPrincipal user);

        Task <OneOf<DeleteProductBacklogItemSucceeded, DeleteProductBacklogItemFailed, ProjectNotFound, ProductBacklogItemNotFound, Forbidden>> DeleteProductBacklogItemAsync(int projectId, int productBacklogItemId, ClaimsPrincipal user);
    }
}
