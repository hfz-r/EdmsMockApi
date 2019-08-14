using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdmsMockApi.Entities
{
    public class ProfileFieldConfiguration : IEntityTypeConfiguration<ProfileField>
    {
        public void Configure(EntityTypeBuilder<ProfileField> builder)
        {
            builder.ToTable("ProfileField");
            builder.HasKey(field => field.Id);

            builder.Property(field => field.ColId).IsRequired();
            builder.Property(field => field.ColDataType).IsRequired();

            builder.HasOne(pf => pf.Profile)
                .WithMany(p => p.ProfileFields)
                .HasForeignKey(pf => pf.ProfileId)
                .IsRequired();
        }
    }
}