namespace DataBase.Models;

public class PaymentInfoModel
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string PlanId { get; set; }
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
    public DateTime CreatedAt { get; set; }
    
}