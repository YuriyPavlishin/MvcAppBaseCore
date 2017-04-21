using BaseApp.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseApp.Data.DataContext.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    internal class UserRoleConfiguration : EntityMappingConfiguration<UserRole>
    {
        public override void Map(EntityTypeBuilder<UserRole> b)
        {
            b.HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
