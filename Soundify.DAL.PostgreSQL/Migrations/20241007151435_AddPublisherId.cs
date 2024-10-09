using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soundify.DAL.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisherId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublisherId",
                table: "Artists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Artists_PublisherId",
                table: "Artists",
                column: "PublisherId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Artists_PublisherId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Artists");
        }
    }
}
