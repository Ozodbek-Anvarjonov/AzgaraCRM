﻿namespace AzgaraCRM.WebUI.Domain.Models;

public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}