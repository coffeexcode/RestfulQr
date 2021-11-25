using Microsoft.EntityFrameworkCore.Migrations;

namespace RestfulQr.Migrations.Migrations
{
    public partial class AddPublicUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicUrl",
                schema: "qr_codes",
                table: "qr_codes",
                newName: "public_url");

            migrationBuilder.AlterColumn<string>(
                name: "public_url",
                schema: "qr_codes",
                table: "qr_codes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "public_url",
                schema: "qr_codes",
                table: "qr_codes",
                newName: "PublicUrl");

            migrationBuilder.AlterColumn<string>(
                name: "PublicUrl",
                schema: "qr_codes",
                table: "qr_codes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
