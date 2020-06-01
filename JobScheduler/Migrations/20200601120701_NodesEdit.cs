using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobScheduler.Migrations
{
    public partial class NodesEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IP",
                table: "Nodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Nodes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int[]>(
                name: "Group",
                table: "Nodes",
                nullable: false,
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPStr",
                table: "Nodes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPStr",
                table: "Nodes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Nodes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int[]>(
                name: "Group",
                table: "Nodes",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(int[]));

            migrationBuilder.AddColumn<IPAddress>(
                name: "IP",
                table: "Nodes",
                type: "inet",
                nullable: true);
        }
    }
}
