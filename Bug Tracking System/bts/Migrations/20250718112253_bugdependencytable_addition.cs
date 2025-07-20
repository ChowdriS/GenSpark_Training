using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bts.Migrations
{
    /// <inheritdoc />
    public partial class bugdependencytable_addition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugDependency_Bugs_ChildBugId",
                table: "BugDependency");

            migrationBuilder.DropForeignKey(
                name: "FK_BugDependency_Bugs_ParentBugId",
                table: "BugDependency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BugDependency",
                table: "BugDependency");

            migrationBuilder.RenameTable(
                name: "BugDependency",
                newName: "BugDependencies");

            migrationBuilder.RenameIndex(
                name: "IX_BugDependency_ChildBugId",
                table: "BugDependencies",
                newName: "IX_BugDependencies_ChildBugId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BugDependencies",
                table: "BugDependencies",
                columns: new[] { "ParentBugId", "ChildBugId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BugDependencies_Bugs_ChildBugId",
                table: "BugDependencies",
                column: "ChildBugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BugDependencies_Bugs_ParentBugId",
                table: "BugDependencies",
                column: "ParentBugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugDependencies_Bugs_ChildBugId",
                table: "BugDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_BugDependencies_Bugs_ParentBugId",
                table: "BugDependencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BugDependencies",
                table: "BugDependencies");

            migrationBuilder.RenameTable(
                name: "BugDependencies",
                newName: "BugDependency");

            migrationBuilder.RenameIndex(
                name: "IX_BugDependencies_ChildBugId",
                table: "BugDependency",
                newName: "IX_BugDependency_ChildBugId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BugDependency",
                table: "BugDependency",
                columns: new[] { "ParentBugId", "ChildBugId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BugDependency_Bugs_ChildBugId",
                table: "BugDependency",
                column: "ChildBugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BugDependency_Bugs_ParentBugId",
                table: "BugDependency",
                column: "ParentBugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
