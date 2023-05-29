using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

namespace It270.MedicalSystem.Common.Infrastructure.Infrastructure.Data.Config.MultiKeyString;

/// <summary>
/// Entity Framework configuration for "KeyStringConfig" entity
/// </summary>
public class KeyStringConfig : IEntityTypeConfiguration<KeyString>
{
    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<KeyString> builder)
    {
        builder.ToTable("key_string", "multilanguage");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(36);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

    }
}