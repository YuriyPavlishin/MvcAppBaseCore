﻿// <auto-generated />
using System;
using BaseApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BaseApp.Data.ProjectMigration.Migrations
{
    [DbContext(typeof(DBData))]
    partial class DBDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.AppLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppVersion")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Callsite")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("ClientIp")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LogLevel")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("LogName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryString")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("RefererUrl")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("RequestContentType")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("RequestMethod")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("RequestUrl")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("UserName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("AppLog", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("GenFileName")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Attachment", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Alpha2")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char(2)");

                    b.Property<string>("Alpha3")
                        .HasMaxLength(3)
                        .HasColumnType("char(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("NumericCode")
                        .HasColumnType("int");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttemptsCount")
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastAttemptDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastAttemptError")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SchedulerId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("ToBccEmailAddresses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCcEmailAddresses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToEmailAddresses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SchedulerId");

                    b.ToTable("NotificationEmail", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("NotificationEmailId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("NotificationEmailId");

                    b.ToTable("NotificationEmailAttachment", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndProcessDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EntityData1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityData2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityData3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityData4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EntityId1")
                        .HasColumnType("int");

                    b.Property<int?>("EntityId2")
                        .HasColumnType("int");

                    b.Property<int?>("EntityId3")
                        .HasColumnType("int");

                    b.Property<int?>("EntityId4")
                        .HasColumnType("int");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSynchronous")
                        .HasColumnType("bit");

                    b.Property<DateTime>("OnDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentSchedulerId")
                        .HasColumnType("int");

                    b.Property<int>("SchedulerActionType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartProcessDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ParentSchedulerId");

                    b.ToTable("Scheduler", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("State", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeletedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("ltrim(rtrim(FirstName + ' ' + LastName))");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ApprovedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApproverIpAddress")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorIpAddress")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("RequestGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserForgotPassword", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Scheduler", "Scheduler")
                        .WithMany("NotificationEmails")
                        .HasForeignKey("SchedulerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Scheduler");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BaseApp.Data.DataContext.Entities.NotificationEmail", "NotificationEmail")
                        .WithMany("NotificationEmailAttachments")
                        .HasForeignKey("NotificationEmailId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("NotificationEmail");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BaseApp.Data.DataContext.Entities.Scheduler", "ParentScheduler")
                        .WithMany("ChildSchedulers")
                        .HasForeignKey("ParentSchedulerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedByUser");

                    b.Navigation("ParentScheduler");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "DeletedByUser")
                        .WithMany("DeletedUsers")
                        .HasForeignKey("DeletedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "UpdatedByUser")
                        .WithMany("UpdatedUsers")
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("DeletedByUser");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "User")
                        .WithMany("UserForgotPasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserRole", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Country", b =>
                {
                    b.Navigation("States");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.Navigation("NotificationEmailAttachments");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.Navigation("ChildSchedulers");

                    b.Navigation("NotificationEmails");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.Navigation("DeletedUsers");

                    b.Navigation("UpdatedUsers");

                    b.Navigation("UserForgotPasswords");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
