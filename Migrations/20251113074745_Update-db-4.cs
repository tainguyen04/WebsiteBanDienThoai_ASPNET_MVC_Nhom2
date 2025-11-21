using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHBanDienThoaiMoi.Migrations
{
    /// <inheritdoc />
    public partial class Updatedb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KhuyenMai",
                table: "ChiTietHoaDonBan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "KhuyenMai",
                table: "ChiTietHoaDonBan",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
