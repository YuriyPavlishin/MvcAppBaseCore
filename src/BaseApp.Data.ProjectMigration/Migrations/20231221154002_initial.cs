using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseApp.Data.ProjectMigration.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ClientIp = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    RequestMethod = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    RequestContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RequestUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    QueryString = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    RefererUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Callsite = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NumericCode = table.Column<int>(type: "int", nullable: true),
                    Alpha2 = table.Column<string>(type: "char(2)", maxLength: 2, nullable: false),
                    Alpha3 = table.Column<string>(type: "char(3)", maxLength: 3, nullable: true),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "ltrim(rtrim(FirstName + ' ' + LastName))"),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GenFileName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scheduler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulerActionType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    ParentSchedulerId = table.Column<int>(type: "int", nullable: true),
                    OnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSynchronous = table.Column<bool>(type: "bit", nullable: false),
                    StartProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityId1 = table.Column<int>(type: "int", nullable: true),
                    EntityId2 = table.Column<int>(type: "int", nullable: true),
                    EntityId3 = table.Column<int>(type: "int", nullable: true),
                    EntityId4 = table.Column<int>(type: "int", nullable: true),
                    EntityData1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityData2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityData3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityData4 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scheduler_Scheduler_ParentSchedulerId",
                        column: x => x.ParentSchedulerId,
                        principalTable: "Scheduler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scheduler_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserForgotPassword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RequestGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ApprovedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApproverIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForgotPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserForgotPassword_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchedulerId = table.Column<int>(type: "int", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCcEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToBccEmailAddresses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttemptsCount = table.Column<int>(type: "int", nullable: false),
                    LastAttemptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttemptError = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationEmail_Scheduler_SchedulerId",
                        column: x => x.SchedulerId,
                        principalTable: "Scheduler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEmailAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationEmailId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEmailAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationEmailAttachment_NotificationEmail_NotificationEmailId",
                        column: x => x.NotificationEmailId,
                        principalTable: "NotificationEmail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CreatedByUserId",
                table: "Attachment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmail_SchedulerId",
                table: "NotificationEmail",
                column: "SchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_AttachmentId",
                table: "NotificationEmailAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEmailAttachment_NotificationEmailId",
                table: "NotificationEmailAttachment",
                column: "NotificationEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_CreatedByUserId",
                table: "Scheduler",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduler_ParentSchedulerId",
                table: "Scheduler",
                column: "ParentSchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryId",
                table: "State",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByUserId",
                table: "User",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserForgotPassword_UserId",
                table: "UserForgotPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLog");

            migrationBuilder.DropTable(
                name: "NotificationEmailAttachment");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "UserForgotPassword");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "NotificationEmail");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Scheduler");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
