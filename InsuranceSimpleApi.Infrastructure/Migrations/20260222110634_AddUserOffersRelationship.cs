using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceSimpleApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOffersRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "InsuranceTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InsuranceTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceTypes_UserId",
                table: "InsuranceTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceTypes_Users_UserId",
                table: "InsuranceTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceTypes_Users_UserId",
                table: "InsuranceTypes");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceTypes_UserId",
                table: "InsuranceTypes");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "InsuranceTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InsuranceTypes");
        }
    }
}
