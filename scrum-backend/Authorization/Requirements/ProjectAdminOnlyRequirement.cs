using Microsoft.AspNetCore.Authorization;
using scrum_backend.Models.Projects;

namespace scrum_backend.Authorization.Requirements
{
    public class ProjectAdminOnlyRequirement : IAuthorizationRequirement
    {
    }
}