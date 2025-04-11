namespace AzgaraCRM.WebUI.Models;

public class PaginationResult<TEntity>
{
    public IEnumerable<TEntity> Data { get; set; }

    public PaginationInfo PaginationInfo { get; set; }
}