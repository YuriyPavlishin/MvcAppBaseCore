﻿// <auto-generated />
using System;
using BaseApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BaseApp.Data.ProjectMigration.Migrations
{
    [DbContext(typeof(DBData))]
    partial class DBDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("GenFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Alpha2")
                        .IsRequired()
                        .HasColumnType("char(2)")
                        .HasMaxLength(2);

                    b.Property<string>("Alpha3")
                        .HasColumnType("char(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("NumericCode")
                        .HasColumnType("int");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("ToBccEmailAddresses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToCcEmailAddresses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToEmailAddresses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SchedulerId");

                    b.ToTable("NotificationEmail");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("NotificationEmailId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("NotificationEmailId");

                    b.ToTable("NotificationEmailAttachment");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Scheduler");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)")
                        .HasMaxLength(16);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("State");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeletedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("ltrim(rtrim(FirstName + ' ' + LastName))");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("UpdatedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ApprovedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApproverIpAddress")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorIpAddress")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<Guid>("RequestGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserForgotPassword");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Scheduler", "Scheduler")
                        .WithMany("NotificationEmails")
                        .HasForeignKey("SchedulerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
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
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
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
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "User")
                        .WithMany("UserForgotPasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
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
                });
#pragma warning restore 612, 618
        }
    }
}
