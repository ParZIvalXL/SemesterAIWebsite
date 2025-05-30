namespace DataBase.Models;

public class PricingModel
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string Name { get; set; }
    public int WordLimit { get; set; }
    public int TemplatesLimit { get; set; }
    public int Languages { get; set; }
}