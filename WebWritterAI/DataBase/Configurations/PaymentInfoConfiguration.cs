using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configurations
{
    public class PaymentInfoConfiguration : IEntityTypeConfiguration<PaymentInfoModel>
    {
        public void Configure(EntityTypeBuilder<PaymentInfoModel> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}