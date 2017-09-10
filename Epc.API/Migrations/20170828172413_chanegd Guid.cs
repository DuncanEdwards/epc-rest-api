using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Epc.API.Migrations
{
    public partial class chanegdGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RememberToken",
                table: "Users",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(Guid),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RememberToken",
                table: "Users",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(Guid),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
