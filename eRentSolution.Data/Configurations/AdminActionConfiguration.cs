using eRentSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eRentSolution.Data.Configurations
{
    public class AdminActionConfiguration : IEntityTypeConfiguration<AdminAction>
    {
        public void Configure(EntityTypeBuilder<AdminAction> builder)
        {
            builder.ToTable("AdminActions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ActionName).HasMaxLength(200).IsRequired();

        }
    }
}
