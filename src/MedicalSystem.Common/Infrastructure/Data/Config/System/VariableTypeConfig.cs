using It270.MedicalSystem.Common.Application.Core.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace It270.MedicalSystem.Common.Infrastructure.Data.Config.System;

/// <summary>
/// Entity Framework configuration for "VariableType" entity
/// </summary>
public class VariableTypeConfig : IEntityTypeConfiguration<VariableType>
{
    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<VariableType> builder)
    {
        builder.ToTable("variable_type", "system");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(45);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}