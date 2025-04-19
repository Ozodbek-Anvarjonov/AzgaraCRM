using AzgaraCRM.WebUI.Domain.Common;
using System.Text.Json.Serialization;

namespace AzgaraCRM.WebUI.Domain.Entities;

public class Food : AuditableEntity
{
    public string Name { get; set; } = default!;

    public double Price { get; set; }

    public int Left { get; set; }

    public string? Path { get; set; }

    public long CategoryId { get; set; }
    public Category Category { get; set; }
}