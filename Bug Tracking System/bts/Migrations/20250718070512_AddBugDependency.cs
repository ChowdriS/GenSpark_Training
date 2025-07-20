using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bts.Migrations
{
    /// <inheritdoc />
    public partial class AddBugDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BugDependency",
                columns: table => new
                {
                    ParentBugId = table.Column<int>(type: "integer", nullable: false),
                    ChildBugId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugDependency", x => new { x.ParentBugId, x.ChildBugId });
                    table.ForeignKey(
                        name: "FK_BugDependency_Bugs_ChildBugId",
                        column: x => x.ChildBugId,
                        principalTable: "Bugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BugDependency_Bugs_ParentBugId",
                        column: x => x.ParentBugId,
                        principalTable: "Bugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BugDependency_ChildBugId",
                table: "BugDependency",
                column: "ChildBugId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugDependency");
        }
    }
}
