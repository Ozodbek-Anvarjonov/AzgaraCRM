namespace AzgaraCRM.WebUI.Models;

public class PaginationInfo
{
    public PaginationInfo(int totalCount, PaginationParameters pagination)
    {
        TotalPages = Convert.ToInt32(Math.Ceiling(totalCount / (decimal)pagination.PageSize));
        CurrentPage = pagination.PageNumber;
    }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public bool HasPrevious => CurrentPage > 1;

    public bool HasNext => CurrentPage < TotalPages;
}