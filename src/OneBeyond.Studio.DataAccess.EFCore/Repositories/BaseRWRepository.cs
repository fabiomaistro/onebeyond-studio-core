using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DelegateDecompiler.EntityFrameworkCore;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using OneBeyond.Studio.Application.SharedKernel.Repositories;
using OneBeyond.Studio.Application.SharedKernel.Specifications;
using OneBeyond.Studio.Domain.SharedKernel.Entities;

namespace OneBeyond.Studio.DataAccess.EFCore.Repositories;

/// <summary>
/// Implements base read-write repository for the <typeparamref name="TAggregateRoot"/> type using EF Core.
/// All the entities returned by this repository are trackable.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TAggregateRoot"></typeparam>
/// <typeparam name="TAggregateRootId"></typeparam>
public class BaseRWRepository<TDbContext, TAggregateRoot, TAggregateRootId>
    : BaseRORepository<TDbContext, TAggregateRoot, TAggregateRootId>
    , IRWRepository<TAggregateRoot, TAggregateRootId>
    where TDbContext : DbContext
    where TAggregateRoot : AggregateRoot<TAggregateRootId>
    where TAggregateRootId : notnull
{
    public BaseRWRepository(
        TDbContext dbContext)
        : base(dbContext)
    {
    }

    public new async Task<TAggregateRoot> GetByIdAsync(
        TAggregateRootId aggregateRootId,
        Includes<TAggregateRoot>? includes,
        CancellationToken cancellationToken)
    {
        var query = await BuildGetQueryAsync(
            (aggregateRoot) => aggregateRoot.Id!.Equals(aggregateRootId),
            includes).ConfigureAwait(false);
        var entity = await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            await EnsureEntityExistsAsync(
                aggregateRootId,
                (aggregateRoot) => aggregateRoot.Id!.Equals(aggregateRootId),
                cancellationToken).ConfigureAwait(false);
        }
        return entity!;
    }

    public async Task<TAggregateRoot> GetByFilterAsync(
        Expression<Func<TAggregateRoot, bool>> filter,
        Includes<TAggregateRoot>? includes,
        CancellationToken cancellationToken)
    {
        var query = await BuildGetQueryAsync(filter, includes).ConfigureAwait(false);
        var entity = await query.SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        if (entity is null)
        {
            await EnsureEntityExistsAsync(default!, filter, cancellationToken).ConfigureAwait(false);
        }
        return entity!;
    }

    public async Task CreateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(aggregateRoot, nameof(aggregateRoot));
        DbSet.Value.Add(aggregateRoot);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(aggregateRoot, nameof(aggregateRoot));
        DbSet.Value.Update(aggregateRoot);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        EnsureArg.IsNotNull(aggregateRoot, nameof(aggregateRoot));
        DbSet.Value.Remove(aggregateRoot);
        await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteAsync(TAggregateRootId id, CancellationToken cancellationToken)
    {
        var aggregate = DbSet.Value.Find(id);

        return aggregate == default
            ? Task.CompletedTask
            : DbSet.Value.DeleteByKeyAsync(cancellationToken, id);
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
        => DbContext.SaveChangesAsync(cancellationToken);

    protected async Task<IQueryable<TAggregateRoot>> BuildGetQueryAsync(
        Expression<Func<TAggregateRoot, bool>> filter,
        Includes<TAggregateRoot>? includes)
    {
        var query = ApplyIncludes(DbSet.Value, includes);
        query = ApplyFiltering(query, filter);
        return query.DecompileAsync();
    }
}
