using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

    public DbSet<PricingModel> Pricings { get; set; }
    public DbSet<UseCaseModel> UseCases { get; set; }
    public DbSet<BlogModel> Blogs { get; set; }
}