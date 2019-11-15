using BaseApp.Data.DataContext.Entities;
using BaseApp.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BaseApp.Data.DataContext
{
    public class DBData : DbContext
    {
        public DBData(DbContextOptions<DBData> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }

            /* delete if not needed
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
            */
            /*TODO: when this https://github.com/aspnet/EntityFramework/issues/214 would be implemented by EF Core
             * modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
               modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
               modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
               modelBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
             */
             
            modelBuilder.ApplyAllConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyDeletableFilter();

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserForgotPassword> UserForgotPasswords { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Scheduler> Schedulers { get; set; }
        public virtual DbSet<NotificationEmail> NotificationEmails { get; set; }
        public virtual DbSet<NotificationEmailAttachment> NotificationEmailAttachments { get; set; }
    }
}
