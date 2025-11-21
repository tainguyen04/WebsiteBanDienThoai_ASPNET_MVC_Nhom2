using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHBanDienThoaiMoi.Migrations
{
    /// <inheritdoc />
    public partial class updateDiaChiNhanHangHoaDonBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChiNhanHang",
                table: "HoaDonBan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChiNhanHang",
                table: "HoaDonBan");
        }
    }
}
