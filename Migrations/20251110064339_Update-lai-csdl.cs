using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHBanDienThoaiMoi.Migrations
{
    /// <inheritdoc />
    public partial class Updatelaicsdl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonNhap_NhaCungCap_NCCId",
                table: "HoaDonNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhieuBaoHanh",
                table: "PhieuBaoHanh");

            migrationBuilder.DropIndex(
                name: "IX_PhieuBaoHanh_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonNhap_NCCId",
                table: "HoaDonNhap");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PhieuBaoHanh");

            migrationBuilder.RenameColumn(
                name: "NCCId",
                table: "HoaDonNhap",
                newName: "TongTien");

            migrationBuilder.AddColumn<int>(
                name: "NhaCungCapId",
                table: "HoaDonNhap",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TongTien",
                table: "HoaDonBan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SanPhamId",
                table: "ChiTietHoaDonNhap",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "HoaDonNhapId",
                table: "ChiTietHoaDonNhap",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChiTietHoaDonBan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "KhuyenMai",
                table: "ChiTietHoaDonBan",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhieuBaoHanh",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuBaoHanh_SanPhamId",
                table: "PhieuBaoHanh",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonNhap_NhaCungCapId",
                table: "HoaDonNhap",
                column: "NhaCungCapId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonNhap_NhaCungCap_NhaCungCapId",
                table: "HoaDonNhap",
                column: "NhaCungCapId",
                principalTable: "NhaCungCap",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" },
                principalTable: "ChiTietHoaDonBan",
                principalColumns: new[] { "HoaDonBanId", "SanPhamId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_HoaDonBan_HoaDonBanId",
                table: "PhieuBaoHanh",
                column: "HoaDonBanId",
                principalTable: "HoaDonBan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_SanPham_SanPhamId",
                table: "PhieuBaoHanh",
                column: "SanPhamId",
                principalTable: "SanPham",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonNhap_NhaCungCap_NhaCungCapId",
                table: "HoaDonNhap");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_HoaDonBan_HoaDonBanId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuBaoHanh_SanPham_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhieuBaoHanh",
                table: "PhieuBaoHanh");

            migrationBuilder.DropIndex(
                name: "IX_PhieuBaoHanh_SanPhamId",
                table: "PhieuBaoHanh");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonNhap_NhaCungCapId",
                table: "HoaDonNhap");

            migrationBuilder.DropColumn(
                name: "NhaCungCapId",
                table: "HoaDonNhap");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HoaDonBan");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChiTietHoaDonBan");

            migrationBuilder.DropColumn(
                name: "KhuyenMai",
                table: "ChiTietHoaDonBan");

            migrationBuilder.RenameColumn(
                name: "TongTien",
                table: "HoaDonNhap",
                newName: "NCCId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PhieuBaoHanh",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "SanPhamId",
                table: "ChiTietHoaDonNhap",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "HoaDonNhapId",
                table: "ChiTietHoaDonNhap",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhieuBaoHanh",
                table: "PhieuBaoHanh",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuBaoHanh_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonNhap_NCCId",
                table: "HoaDonNhap",
                column: "NCCId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonNhap_NhaCungCap_NCCId",
                table: "HoaDonNhap",
                column: "NCCId",
                principalTable: "NhaCungCap",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuBaoHanh_ChiTietHoaDonBan_HoaDonBanId_SanPhamId",
                table: "PhieuBaoHanh",
                columns: new[] { "HoaDonBanId", "SanPhamId" },
                principalTable: "ChiTietHoaDonBan",
                principalColumns: new[] { "HoaDonBanId", "SanPhamId" });
        }
    }
}
