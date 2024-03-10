using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using OneBeyond.Studio.Domain.SharedKernel.Entities;

namespace OneBeyond.Studio.Application.SharedKernel.Repositories;

public static class IRORepositoryExtensions
{
    public static Task<TEntity> GetByIdAsync<TEntity, TEntityId>(
        this IRORepository<TEntity, TEntityId> roRepository,
        TEntityId entityId,
        CancellationToken cancellationToken)
        where TEntity : DomainEntity<TEntityId>
        where TEntityId : notnull
    {
        EnsureArg.IsNotNull(roRepository, nameof(roRepository));

        return roRepository.GetByIdAsync(
            entityId,
            default,
            cancellationToken);
    }
}
