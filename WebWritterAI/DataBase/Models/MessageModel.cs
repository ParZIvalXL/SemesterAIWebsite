namespace DataBase.Models;

public class MessageModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }
}