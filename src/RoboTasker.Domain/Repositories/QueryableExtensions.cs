using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Repositories;

public static class QueryableExtensions
{
    private static async Task<PaginatedList<T>> CreateAsync<T>(
        this IQueryable<T> queryable, 
        int page, int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var totalItems = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PaginatedList<T>(items, totalItems, page, pageSize);
    }
    
    public static Task<PaginatedList<T>> PaginateAsync<T>(
        this IQueryable<T> queryable,
        int page, int pageSize,
        CancellationToken cancellationToken = default) 
        => queryable.CreateAsync(page, pageSize, cancellationToken);
}