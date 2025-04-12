using AzgaraCRM.WebUI.Domain.Enums;
using System.Text.Json.Serialization;

namespace AzgaraCRM.WebUI.Models.Users;

public class UserViewModel
{
    public long Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}