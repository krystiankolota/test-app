using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WeatherDisplayer.Application.Common.Interfaces;
using WeatherDisplayer.Domain.Models;
using WeatherDisplayer.Infrastructure.Database;
namespace WeatherDisplayer.Infrastructure.Services;

public class BaseRepository<TEntity>
    : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly WeatherDataContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(WeatherDataContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task SaveAsync()
    {
        return Context.SaveChangesAsync();
    }

    public async Task<IList<TEntity>> GetAsync(Func<WeatherData, bool> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool asNoTracking = false)
    {
        var query = GetQuery(filter, orderBy);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        bool asNoTracking = false)
    {
        var query = GetQuery(filter, null);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<TModel>> ExecuteStoredProcedureAsync<TModel>(string storedProcedureName, string parameterName, string parameterValue)
        where TModel : class
    {
        var command = $"EXEC {storedProcedureName} @{parameterName}={parameterValue}";
        var results = await Context.Set<TModel>()
            .FromSqlRaw(command, parameterValue)
            .ToListAsync();

        return results;
    }

    private IQueryable<TEntity> GetQuery(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }
}
