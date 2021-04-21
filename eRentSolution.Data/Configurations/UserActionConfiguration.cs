using eRentSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eRentSolution.Data.Configurations
{
    public class UserActionConfiguration : IEntityTypeConfiguration<UserAction>
    {
        public void Configure(EntityTypeBuilder<UserAction> builder)
        {
            builder.ToTable("UserActions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ActionName).HasMaxLength(200).IsRequired();

        }
    }
}
