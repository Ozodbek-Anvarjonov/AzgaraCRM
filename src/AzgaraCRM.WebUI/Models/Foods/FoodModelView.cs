using AzgaraCRM.WebUI.Models.Categories;

namespace AzgaraCRM.WebUI.Models.Foods;

public class FoodModelView
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int Left { get; set; }

    public string? Path { get; set; }

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;
}