using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class WorkoutEventAttendeeInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutEventAttendees",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHost = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutEventAttendees", x => new { x.AppUserId, x.WorkoutEventId });
                    table.ForeignKey(
                        name: "FK_WorkoutEventAttendees_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutEventAttendees_WorkoutEvents_WorkoutEventId",
                        column: x => x.WorkoutEventId,
                        principalTable: "WorkoutEvents",
                        principalColumn: "WorkoutEvent_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutEventAttendees_WorkoutEventId",
                table: "WorkoutEventAttendees",
                column: "WorkoutEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutEventAttendees");
        }
    }
}
