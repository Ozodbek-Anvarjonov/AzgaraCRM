namespace AzgaraCRM.WebUI.Domain.Common;

public class AuditableEntity
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public long? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }
    public long? DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}