using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseApp.Data.ProjectMigration.Migrations
{
    public partial class addAppLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogName = table.Column<string>(maxLength: 128, nullable: true),
                    LogLevel = table.Column<string>(maxLength: 64, nullable: true),
                    LogDate = table.Column<DateTime>(nullable: true),
                    AppVersion = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    ClientIp = table.Column<string>(maxLength: 64, nullable: true),
                    RequestMethod = table.Column<string>(maxLength: 32, nullable: true),
                    RequestContentType = table.Column<string>(maxLength: 256, nullable: true),
                    RequestUrl = table.Column<string>(maxLength: 1024, nullable: true),
                    QueryString = table.Column<string>(maxLength: 2048, nullable: true),
                    RefererUrl = table.Column<string>(maxLength: 1024, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 1024, nullable: true),
                    Callsite = table.Column<string>(maxLength: 512, nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLog");
        }
    }
}
