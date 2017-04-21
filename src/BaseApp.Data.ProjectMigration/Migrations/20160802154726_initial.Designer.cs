using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BaseApp.Data.DataContext;

namespace BaseApp.Data.ProjectMigration.Migrations
{
    [DbContext(typeof(DBData))]
    [Migration("20160802154726_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<long>("FileSize");

                    b.Property<string>("GenFileName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 512);

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Country", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Alpha2")
                        .IsRequired()
                        .HasColumnType("char(2)")
                        .HasAnnotation("MaxLength", 2);

                    b.Property<string>("Alpha3")
                        .HasColumnType("char(3)")
                        .HasAnnotation("MaxLength", 3);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("NumericCode");

                    b.Property<int>("Ordinal");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttemptsCount");

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("LastAttemptDate");

                    b.Property<string>("LastAttemptError");

                    b.Property<DateTime?>("ProcessedDate");

                    b.Property<int>("SchedulerId");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<string>("ToBccEmailAddresses");

                    b.Property<string>("ToCcEmailAddresses");

                    b.Property<string>("ToEmailAddresses")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SchedulerId");

                    b.ToTable("NotificationEmail");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttachmentId");

                    b.Property<int>("NotificationEmailId");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("NotificationEmailId");

                    b.ToTable("NotificationEmailAttachment");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Role", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime?>("EndProcessDate");

                    b.Property<string>("EntityData1");

                    b.Property<string>("EntityData2");

                    b.Property<string>("EntityData3");

                    b.Property<string>("EntityData4");

                    b.Property<int?>("EntityId1");

                    b.Property<int?>("EntityId2");

                    b.Property<int?>("EntityId3");

                    b.Property<int?>("EntityId4");

                    b.Property<string>("ErrorMessage");

                    b.Property<bool>("IsSynchronous");

                    b.Property<DateTime>("OnDate");

                    b.Property<int?>("ParentSchedulerId");

                    b.Property<int>("SchedulerActionType");

                    b.Property<DateTime?>("StartProcessDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ParentSchedulerId");

                    b.ToTable("Scheduler");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 16);

                    b.Property<int>("CountryId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("State");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int?>("DeletedByUserId");

                    b.Property<DateTime?>("DeletedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("ltrim(rtrim(FirstName + ' ' + LastName))");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("UpdatedByUserId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("DeletedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ApprovedDateTime");

                    b.Property<string>("ApproverIpAddress")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorIpAddress")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<Guid>("RequestGuid");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserForgotPassword");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Attachment", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmail", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Scheduler", "Scheduler")
                        .WithMany("NotificationEmails")
                        .HasForeignKey("SchedulerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.NotificationEmailAttachment", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BaseApp.Data.DataContext.Entities.NotificationEmail", "NotificationEmail")
                        .WithMany("NotificationEmailAttachments")
                        .HasForeignKey("NotificationEmailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.Scheduler", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BaseApp.Data.DataContext.Entities.Scheduler", "ParentScheduler")
                        .WithMany("ChildSchedulers")
                        .HasForeignKey("ParentSchedulerId");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.State", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.User", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "DeletedByUser")
                        .WithMany("DeletedUsers")
                        .HasForeignKey("DeletedByUserId");

                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "UpdatedByUser")
                        .WithMany("UpdatedUsers")
                        .HasForeignKey("UpdatedByUserId");
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserForgotPassword", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "User")
                        .WithMany("UserForgotPasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BaseApp.Data.DataContext.Entities.UserRole", b =>
                {
                    b.HasOne("BaseApp.Data.DataContext.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BaseApp.Data.DataContext.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
