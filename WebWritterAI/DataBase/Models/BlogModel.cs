namespace DataBase.Models;

public class BlogModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string UserAvatar { get; set; }
    public string UserName { get; set; }
    public DateTime Date { get; set; }
    public int TimeToRead { get; set; }
    public string Image { get; set; }
}