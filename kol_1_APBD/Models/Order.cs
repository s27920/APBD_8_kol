namespace kol_1_APBD.Models;

public class Order
{
    public int IdOrder { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public int IdClient { get; set; }
    public IEnumerable<Product> Products{ get; set; }

}