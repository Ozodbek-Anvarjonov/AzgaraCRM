using AzgaraCRM.WebUI.Domain.Common;
using System.Text.Json.Serialization;

namespace AzgaraCRM.WebUI.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = default!;

    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    public IEnumerable<Food> Foods { get; set; } = new List<Food>();
}