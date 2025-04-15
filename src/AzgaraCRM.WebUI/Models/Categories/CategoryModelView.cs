using AzgaraCRM.WebUI.Models.Foods;

namespace AzgaraCRM.WebUI.Models.Categories;

public class CategoryModelView
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public DateTime LastModified { get; set; }

    public IEnumerable<FoodModelView> Foods { get; set; }
}