using Microsoft.EntityFrameworkCore.Migrations;

namespace JobScheduler.Migrations
{
    public partial class Schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Schedules",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Nodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IPStr",
                table: "Nodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int[]>(
                name: "Group",
                table: "Nodes",
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_JobId",
                table: "Schedules",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Jobs_JobId",
                table: "Schedules",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Jobs_JobId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_JobId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Nodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IPStr",
                table: "Nodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int[]>(
                name: "Group",
                table: "Nodes",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(int[]),
                oldNullable: true);
        }
    }
}
