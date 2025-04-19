namespace AzgaraCRM.WebUI.Models.Foods;

public class CreateFoodModel
{
    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int Left { get; set; }

    public long CategoryId { get; set; }

    public IFormFile? File { get; set; }
}