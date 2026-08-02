using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SniffAndStay.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangePetTypeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Pets",
                newName: "PetType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PetType",
                table: "Pets",
                newName: "Type");
        }
    }
}
