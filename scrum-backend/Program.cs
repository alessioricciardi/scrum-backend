using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using scrum_backend.Authorization.Handlers;
using scrum_backend.Authorization.Policies;
using scrum_backend.Authorization.Requirements;
using scrum_backend.Authorization.Services;
using scrum_backend.Data;
using scrum_backend.Models.AppUsers;
using scrum_backend.Models.Projects;
using scrum_backend.Services.AuthService;
using scrum_backend.Services.ProjectMemberService;
using scrum_backend.Services.ProjectProductBacklogService;
using scrum_backend.Services.ProjectService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// SQLite Database Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString")));

// Authentication
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Resource Based Authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.ProjectAdminOnly, policy =>
        policy.AddRequirements(new ProjectAdminOnlyRequirement()))

    .AddPolicy(AuthorizationPolicies.ProjectMemberOnly, policy =>
        policy.AddRequirements(new ProjectMemberOnlyRequirement()))

    .AddPolicy(AuthorizationPolicies.ProductOwnerOnly, policy =>
        policy.AddRequirements(new ProjectRoleOnlyRequirement(ProjectMemberRole.ProductOwner)))

    .AddPolicy(AuthorizationPolicies.ScrumMasterOnly, policy =>
        policy.AddRequirements(new ProjectRoleOnlyRequirement(ProjectMemberRole.ScrumMaster)))

    .AddPolicy(AuthorizationPolicies.DeveloperOnly, policy =>
        policy.AddRequirements(new ProjectRoleOnlyRequirement(ProjectMemberRole.Developer)));


// Cookie Same Site = Strict
builder.Services.ConfigureApplicationCookie(configure =>
{
    configure.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
});

// Auto Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["AutoMapperLicenseKey"];
}
, typeof(Program));

// Service DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthorizationHandler, ProjectAdminOnlyAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ProjectRoleOnlyAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ProjectMemberOnlyAuthorizationHandler>();
builder.Services.AddScoped<IProjectAccessService,  ProjectAccessService>();


builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectMemberService, ProjectMemberService>();
builder.Services.AddScoped<IProjectProductBacklogService, ProjectProductBacklogService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
