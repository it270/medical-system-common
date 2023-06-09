using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

namespace It270.MedicalSystem.Common.Infrastructure.Infrastructure.Data.Config.MultiLanguage;

/// <summary>
/// Entity Framework configuration for "StringTemplateConfig" entity
/// </summary>
public class StringTemplateConfig : IEntityTypeConfiguration<StringTemplate>
{

    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<StringTemplate> builder)
    {
        builder.ToTable("string_template", "multilanguage");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .IsRequired();

        builder.Property(e => e.KeyStringId)
            .HasColumnName("key_string_id")
            .IsRequired();

        builder.Property(e => e.LanguageId)
            .HasColumnName("language_id")
            .IsRequired();

        builder.HasOne(e => e.Language)
            .WithMany(p => p.StringTemplates)
            .HasForeignKey(e => e.LanguageId);

        builder.HasOne(e => e.KeyString)
           .WithMany(p => p.StringTemplates)
           .HasForeignKey(e => e.KeyStringId);

    }
}