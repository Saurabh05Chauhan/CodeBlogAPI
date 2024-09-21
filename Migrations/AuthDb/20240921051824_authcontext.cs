using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBlogAPI.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class authcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "49aa06f4-10ed-4309-b4a1-4fb2f8511cf3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "898c037a-52e5-46fe-be7a-f15ac0eab8bc", "AQAAAAIAAYagAAAAEEzPaJ2gDmW3GxaYdFacwJcgsQ+ZKueWl39Z0702jHALZkJbwEw5wm99hjiROBY2zg==", "4fcf11a3-3e18-4425-82f4-0f59760ed50d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "49aa06f4-10ed-4309-b4a1-4fb2f8511cf3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e42c4cbd-d28d-4bad-8675-30be35230d00", "AQAAAAIAAYagAAAAEMkN7zp/dYtC/+jA13QKByGQ+SBtTuQGdN9i54FJZznIr+cwdRp2BZI6IM60rilp4A==", "5717a034-f932-4382-85cd-522f3fb4417b" });
        }
    }
}
