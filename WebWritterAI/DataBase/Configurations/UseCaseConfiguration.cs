using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configurations;

public class UseCaseConfiguration : IEntityTypeConfiguration<UseCaseModel>
{
    public void Configure(EntityTypeBuilder<UseCaseModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}