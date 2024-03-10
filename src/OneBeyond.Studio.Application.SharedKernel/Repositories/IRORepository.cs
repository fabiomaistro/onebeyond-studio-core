using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using OneBeyond.Studio.Application.SharedKernel.Specifications;
using OneBeyond.Studio.Domain.SharedKernel.Entities;
using OneBeyond.Studio.Domain.SharedKernel.Specifications;

namespace OneBeyond.Studio.Application.SharedKernel.Repositories;

/// <summary>
/// Represents a repository with read-only operations.
/// </summary>
public interface IRORepository<TEntity>
{
    Task<IReadOnlyCollection<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = default,
        Includes<TEntity>? includes = default,
        Paging? paging = default,
        IReadOnlyCollection<Sorting<TEntity>>? sortings = default,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TResultDto>> ListAsync<TResultDto>(
        Expression<Func<TEntity, TResultDto>> projection,
        Expression<Func<TEntity, bool>>? filter = null,
        Paging? paging = null,
        IReadOnlyCollection<Sorting<TEntity>>? sortings = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filter = default,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? filter = default,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a repository with read-only operations.
/// </summary>
public interface IRORepository<TEntity, TEntityId> : IRORepository<TEntity>
    where TEntity : DomainEntity<TEntityId>
    where TEntityId : notnull
{
    Task<TEntity> GetByIdAsync(
        TEntityId entityId,
        Includes<TEntity>? includes,
        CancellationToken cancellationToken);

    Task<TResultDto> GetByIdAsync<TResultDto>(
        TEntityId entityId,
        Expression<Func<TEntity, TResultDto>> projection,
        CancellationToken cancellationToken);
}
