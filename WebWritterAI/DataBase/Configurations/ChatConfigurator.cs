using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configurations;

public class ChatConfigurator : IEntityTypeConfiguration<ChatModel>
{
    public void Configure(EntityTypeBuilder<ChatModel> builder)
    {
        builder.HasKey(x => x.Id);
    }
}