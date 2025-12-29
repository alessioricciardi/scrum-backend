using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace scrum_backend.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SprintBacklogItemId",
                table: "ProjectMembers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductBacklogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    StoryPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    DateCreated = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBacklogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBacklogItems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprints_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SprintBacklogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SprintId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintBacklogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SprintBacklogItems_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_SprintBacklogItemId",
                table: "ProjectMembers",
                column: "SprintBacklogItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItems_ProjectId",
                table: "ProductBacklogItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintBacklogItems_SprintId",
                table: "SprintBacklogItems",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ProjectId",
                table: "Sprints",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_SprintBacklogItems_SprintBacklogItemId",
                table: "ProjectMembers",
                column: "SprintBacklogItemId",
                principalTable: "SprintBacklogItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_SprintBacklogItems_SprintBacklogItemId",
                table: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProductBacklogItems");

            migrationBuilder.DropTable(
                name: "SprintBacklogItems");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_SprintBacklogItemId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "SprintBacklogItemId",
                table: "ProjectMembers");
        }
    }
}
