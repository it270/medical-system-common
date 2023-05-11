using It270.MedicalSystem.Common.Application.Core.Entities.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace It270.MedicalSystem.Common.Infrastructure.Data.Config.System;

/// <summary>
/// Entity Framework configuration for "Variable" entity
/// </summary>
public class VariableConfig : IEntityTypeConfiguration<Variable>
{
    /// <summary>
    /// Configure entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Variable> builder)
    {
        builder.ToTable("variable", "system");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(45);

        builder.Property(e => e.Value)
            .HasColumnName("value")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(150);

        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(e => e.TypeId)
            .HasColumnName("type_id")
            .IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(p => p.Variables)
            .HasForeignKey(e => e.CategoryId);

        builder.HasOne(e => e.Type)
            .WithMany(p => p.Variables)
            .HasForeignKey(e => e.TypeId);

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}