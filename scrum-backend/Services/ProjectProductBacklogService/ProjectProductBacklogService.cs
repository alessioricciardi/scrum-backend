

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OneOf;
using scrum_backend.Authorization.Policies;
using scrum_backend.Authorization.Services;
using scrum_backend.Data;
using scrum_backend.Dtos.ProjectProductBacklog.Requests;
using scrum_backend.Dtos.ProjectProductBacklog.Responses;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectProductBacklogService
{
    public class ProjectProductBacklogService : IProjectProductBacklogService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IProjectAccessService _projectAccessService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectProductBacklogService> _logger;

        public ProjectProductBacklogService(
            AppDbContext appDbContext, IProjectAccessService projectAccessService,
            IMapper mapper, ILogger<ProjectProductBacklogService> logger)
        {
            _appDbContext = appDbContext;
            _projectAccessService = projectAccessService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OneOf<GetProductBacklogItemsSucceeded, ProjectNotFound, Forbidden>> GetProductBacklogItemsAsync(int projectId, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProjectMemberOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var productBacklogItmes = await _appDbContext.ProductBacklogItems
                    .Where(i => i.ProjectId == projectId)
                    .ToListAsync();

            var response = _mapper.Map<ICollection<GetProductBacklogItemResponseDto>>(productBacklogItmes);

            return new GetProductBacklogItemsSucceeded(response);
        }


        public async Task<OneOf<GetProductBacklogItemByIdSucceeded, ProjectNotFound, ProductBacklogItemNotFound, Forbidden>> GetProductBacklogItemByIdAsync(int projectId, int productBacklogItemId, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProjectMemberOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var productBacklogItem = await _appDbContext.ProductBacklogItems.FindAsync(productBacklogItemId);

            if (productBacklogItem is null)
                return new ProductBacklogItemNotFound();

            var result = _mapper.Map<GetProductBacklogItemResponseDto>(productBacklogItem);

            return new GetProductBacklogItemByIdSucceeded(result);
        }

        public async Task<OneOf<CreateProductBacklogItemSucceeded, CreateProductBacklogItemFailed, ProjectNotFound, InvalidType, Forbidden>> CreateProductBacklogItemAsync(int projectId, CreateProductBacklogItemRequestDto createItemDto, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProductOwnerOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            if (!Enum.IsDefined<ProductBacklogItemType>(createItemDto.Type))
                return new InvalidType("Invalid Product Backlog item type.");

            if (createItemDto.Type == ProductBacklogItemType.UserStory
                && createItemDto.StoryPoints is null)
            {
                return new InvalidType("Story Points are required for User Stories and must not be set for other backlog item types."
);
            }

            if(createItemDto.Type != ProductBacklogItemType.UserStory
                && createItemDto.StoryPoints is not null)
            {
                return new InvalidType("Story Points are required for User Stories and must not be set for other backlog item types."
);
            }

            var productBacklogItem = new ProductBacklogItem
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Type = createItemDto.Type,
                ProjectId = projectId,
                Priority = createItemDto.Priority,
                StoryPoints = createItemDto.StoryPoints,
            };

            _appDbContext.ProductBacklogItems.Add(productBacklogItem);

            try
            {
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<CreateProductBacklogItemResponseDto>(productBacklogItem);
                return new CreateProductBacklogItemSucceeded(response);
            }
            catch
            {
                return new CreateProductBacklogItemFailed();
            }
        }

        public async Task<OneOf<UpdateProductBacklogItemSucceeded, UpdateProductBacklogItemFailed, ProjectNotFound, ProductBacklogItemNotFound, InvalidType, Forbidden>> UpdateProductBacklogItemAsync(int projectId, int productBacklogItemId, UpdateProductBacklogItemRequestDto updateItemDto, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProductOwnerOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var productBacklogItem = await _appDbContext.ProductBacklogItems
                                               .FirstOrDefaultAsync(i =>
                                                    i.Id == productBacklogItemId &&
                                                    i.ProjectId == projectId 
                                               );

            if (productBacklogItem is null)
                return new ProductBacklogItemNotFound();

            if (!Enum.IsDefined<ProductBacklogItemType>(updateItemDto.Type))
                return new InvalidType("Invalid Product Backlog item type.");

            productBacklogItem.Name = updateItemDto.Name;
            productBacklogItem.Description = updateItemDto.Description;
            productBacklogItem.Type = updateItemDto.Type;
            productBacklogItem.Status = updateItemDto.Status;
            productBacklogItem.Priority = updateItemDto.Priority;
            productBacklogItem.StoryPoints = updateItemDto.StoryPoints;

            try
            {
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<UpdateProductBacklogItemResponseDto>(productBacklogItem);
                return new UpdateProductBacklogItemSucceeded(response);
            }
            catch
            {
                return new UpdateProductBacklogItemFailed();
            }

        }

        public async Task<OneOf<DeleteProductBacklogItemSucceeded, DeleteProductBacklogItemFailed, ProjectNotFound, ProductBacklogItemNotFound, Forbidden>> DeleteProductBacklogItemAsync(int projectId, int productBacklogItemId, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProductOwnerOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var productBacklogItem = await _appDbContext.ProductBacklogItems
                                   .FirstOrDefaultAsync(i =>
                                        i.Id == productBacklogItemId &&
                                        i.ProjectId == projectId
                                   );

            if (productBacklogItem is null)
                return new ProductBacklogItemNotFound();

            _appDbContext.Remove(productBacklogItem);

            try
            {
                await _appDbContext.SaveChangesAsync();

                return new DeleteProductBacklogItemFailed();
            }
            catch
            {
                return new DeleteProductBacklogItemFailed();
            }
        }
    }
}
