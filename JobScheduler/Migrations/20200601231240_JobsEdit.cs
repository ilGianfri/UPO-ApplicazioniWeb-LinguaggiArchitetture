using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobScheduler.Migrations
{
    public partial class JobsEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Jobs_JobId",
                table: "Nodes");

            migrationBuilder.DropIndex(
                name: "IX_Nodes_JobId",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Nodes");

            migrationBuilder.AddColumn<string[]>(
                name: "Nodes",
                table: "Jobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nodes",
                table: "Jobs");

            migrationBuilder.AddColumn<decimal>(
                name: "JobId",
                table: "Nodes",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nodes_JobId",
                table: "Nodes",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Jobs_JobId",
                table: "Nodes",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
