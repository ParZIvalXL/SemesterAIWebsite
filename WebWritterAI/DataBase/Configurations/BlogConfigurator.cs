using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configurations;

public class BlogConfigurator : IEntityTypeConfiguration<BlogModel>
{
    public void Configure(EntityTypeBuilder<BlogModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}