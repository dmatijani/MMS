using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileColumnToPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Payments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Payments");
        }
    }
}
