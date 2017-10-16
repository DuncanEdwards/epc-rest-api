using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Epc.API.Migrations
{
    public partial class AddedanIsActivatedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpcTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "EpcTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpcTypes", x => x.Id);
                });
        }
    }
}
