using System;
using System.Collections.Generic;

public class IndexViewModel
{
    //public IEnumerable<string> Items { get; set; }
    public Pager Pager { get; set; }
}
public class Pager
{
    public Pager(long totalItems, long? page, long pageSize = 10)
    {
        // calculate total, start and end pages
        var totalPages = (long)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        var currentPage = page != null ? (long)page : 1;
        var startPage = currentPage - 5;
        var endPage = currentPage + 4;
        if (startPage <= 0)
        {
            endPage -= (startPage - 1);
            startPage = 1;
        }
        if (endPage > totalPages)
        {
            endPage = totalPages;
            if (endPage > 10)
            {
                startPage = endPage - 9;
            }
        }

        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
    }

    public long TotalItems { get; private set; }
    public long CurrentPage { get; private set; }
    public long PageSize { get; private set; }
    public long TotalPages { get; private set; }
    public long StartPage { get; private set; }
    public long EndPage { get; private set; }
}