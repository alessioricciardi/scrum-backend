using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scrum_backend.Dtos;
using scrum_backend.Dtos.ProjectProductBacklog.Requests;
using scrum_backend.Results;
using scrum_backend.Services.ProjectProductBacklogService;


namespace scrum_backend.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/product-backlog-items")]
    public class ProjectProductBacklogController : ControllerBase
    {
        private readonly IProjectProductBacklogService _productBacklogService;

        public ProjectProductBacklogController(IProjectProductBacklogService productBacklogService)
        {
            _productBacklogService = productBacklogService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProductBacklogItems([FromRoute] int projectId)
        {
            var result = await _productBacklogService.GetProductBacklogItemsAsync(projectId, User);

            return result.Match<IActionResult>(
                getProductBacklogItemsSucceeded => Ok(getProductBacklogItemsSucceeded.GetProductBacklogItemsDto),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                forbidden => Forbid()
            );
        }

        [Authorize]
        [HttpGet("{productBacklogItemId:int}")]
        public async Task<IActionResult> GetProductBacklogItemById([FromRoute] int projectId, int productBacklogItemId)
        {
            var result = await _productBacklogService.GetProductBacklogItemByIdAsync(projectId, productBacklogItemId, User);

            return result.Match<IActionResult>(
                getProductBacklogItemByIdSucceeded => Ok(getProductBacklogItemByIdSucceeded.GetProductBacklogItemDto),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                productBacklogItemNotFound => NotFound(new ErrorResponseDto { Message = "Product backlog item not found." }),
                forbidden => Forbid()
            );
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProductBacklogItem([FromRoute] int projectId, [FromBody] CreateProductBacklogItemRequestDto createProductBacklogItemRequestDto)
        {
            var result = await _productBacklogService.CreateProductBacklogItemAsync(projectId, createProductBacklogItemRequestDto, User);

            return result.Match<IActionResult>(
                createProductBacklogItemSucceeded =>
                {
                    var dto = createProductBacklogItemSucceeded.CreateProductBacklogItemResponseDto;
                    return CreatedAtAction(
                        nameof(GetProductBacklogItemById),
                        new { projectId, productBacklogItemId = dto.Id },
                        dto);
                },
                createProductBacklogItemFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to create product backlog item." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                invalidType => BadRequest(new ErrorResponseDto { Message = "Failed to create a product backlog item.", Errors = [invalidType.Error]}),
                forbidden => Forbid()
            );
        }

        [Authorize]
        [HttpPut("{productBacklogItemId:int}")]
        public async Task<IActionResult> UpdateProductBacklogItem([FromRoute] int projectId, int productBacklogItemId, [FromBody] UpdateProductBacklogItemRequestDto updateProductBacklogItemRequestDto)
        {
            var result = await _productBacklogService.UpdateProductBacklogItemAsync(projectId, productBacklogItemId, updateProductBacklogItemRequestDto, User);

            return result.Match<IActionResult>(
                updateProductBacklogItemSucceeded => Ok(updateProductBacklogItemSucceeded.UpdateProductBacklogItemResponseDto),
                updateProductBacklogItemFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to update product backlog item." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                productBacklogItemNotFound => NotFound(new ErrorResponseDto { Message = "Product backlog item not found." }),
                invalidType => BadRequest(new ErrorResponseDto { Message = "Failed to create a product backlog item.", Errors = [invalidType.Error] }),
                forbidden => Forbid()
            );
        }

        [Authorize]
        [HttpDelete("{projectBacklogItemId:int}")]
        public async Task<IActionResult> DeleteProductBacklogItem([FromRoute] int projectId, int projectBacklogItemId)
        {
            var result = await _productBacklogService.DeleteProductBacklogItemAsync(projectId, projectBacklogItemId, User);

            return result.Match<IActionResult>(
                deleteProductBacklogItemSucceeded => NoContent(),
                deleteProductBacklogItemFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to delete product backlog item." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                productBacklogItemNotFound => NotFound(new ErrorResponseDto { Message = "Product backlog item not found." }),
                forbidden => Forbid()
            );
        }
    }
}
