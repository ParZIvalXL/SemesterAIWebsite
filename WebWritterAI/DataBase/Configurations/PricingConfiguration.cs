using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configurations;

public class PricingConfiguration : IEntityTypeConfiguration<PricingModel>
{
    public void Configure(EntityTypeBuilder<PricingModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}