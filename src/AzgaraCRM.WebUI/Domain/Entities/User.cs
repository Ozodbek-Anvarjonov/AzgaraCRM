using AzgaraCRM.WebUI.Domain.Common;
using AzgaraCRM.WebUI.Domain.Enums;

namespace AzgaraCRM.WebUI.Domain.Entities;

public class User : AuditableEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public UserRole Role { get; set; }

    public bool IsDisabled { get; set; }
}