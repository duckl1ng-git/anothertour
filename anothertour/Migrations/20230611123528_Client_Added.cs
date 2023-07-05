using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anothertour.Migrations
{
    /// <inheritdoc />
    public partial class Client_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalOrdersCost = table.Column<int>(type: "int", nullable: false),
                    OrdersNumber = table.Column<int>(type: "int", nullable: false),
                    ReviewsNumber = table.Column<int>(type: "int", nullable: false),
                    CurrentDiscount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
