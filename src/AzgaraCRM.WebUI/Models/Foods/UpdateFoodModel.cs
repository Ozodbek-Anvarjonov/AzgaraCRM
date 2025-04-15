namespace AzgaraCRM.WebUI.Models.Foods;

public class UpdateFoodModel
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int Left { get; set; }

    public long CategoryId { get; set; }
}