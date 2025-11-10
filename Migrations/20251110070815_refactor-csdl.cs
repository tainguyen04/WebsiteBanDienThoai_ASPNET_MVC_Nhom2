using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHBanDienThoaiMoi.Migrations
{
    /// <inheritdoc />
    public partial class refactorcsdl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" },
                principalTable: "ChiTietHoaDonBan",
                principalColumns: new[] { "HoaDonBanId", "SanPhamId" },
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" },
                principalTable: "ChiTietHoaDonBan",
                principalColumns: new[] { "HoaDonBanId", "SanPhamId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
