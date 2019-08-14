using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdmsMockApi.Entities
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profile");
            builder.HasKey(profile => profile.Id);

            builder.Property(profile => profile.ProfileId).IsRequired();
            builder.Property(profile => profile.ProfileName).HasMaxLength(2000).IsRequired();
        }
    }
}