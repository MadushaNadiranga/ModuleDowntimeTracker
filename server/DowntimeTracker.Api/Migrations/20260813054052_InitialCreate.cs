using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DowntimeTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Silo = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlannedDowntimeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PlannedStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    PlannedEndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ActualStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    SavedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedDowntimeHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnplannedDowntimeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Style = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    SavedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnplannedDowntimeHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReasonText = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reasons_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Heads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Heads_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlannedDowntimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PlannedStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    PlannedEndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    ActualStartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedDowntimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedDowntimes_Heads_HeadId",
                        column: x => x.HeadId,
                        principalTable: "Heads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnplannedDowntimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Style = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnplannedDowntimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnplannedDowntimes_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnplannedDowntimes_Heads_HeadId",
                        column: x => x.HeadId,
                        principalTable: "Heads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnplannedDowntimes_Reasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "Reasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Heads_ModuleId",
                table: "Heads",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedDowntimes_HeadId",
                table: "PlannedDowntimes",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Reasons_DepartmentId",
                table: "Reasons",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedDowntimes_DepartmentId",
                table: "UnplannedDowntimes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedDowntimes_HeadId",
                table: "UnplannedDowntimes",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedDowntimes_ReasonId",
                table: "UnplannedDowntimes",
                column: "ReasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedDowntimeHistories");

            migrationBuilder.DropTable(
                name: "PlannedDowntimes");

            migrationBuilder.DropTable(
                name: "UnplannedDowntimeHistories");

            migrationBuilder.DropTable(
                name: "UnplannedDowntimes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Heads");

            migrationBuilder.DropTable(
                name: "Reasons");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
