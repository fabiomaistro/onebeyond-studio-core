using OneBeyond.Studio.DataAccess.EFCore.Repositories;
using OneBeyond.Studio.Domain.SharedKernel.Entities;

namespace OneBeyond.Studio.DataAccess.EFCore.Tests.Data.Repositories;

internal class RWRepository<TAggregateRoot, TAggregateRootId>
    : BaseRWRepository<DbContexts.DbContext, TAggregateRoot, TAggregateRootId>
    where TAggregateRoot : AggregateRoot<TAggregateRootId>
    where TAggregateRootId : notnull
{
    public RWRepository(
        DbContexts.DbContext dbContext)
        : base(dbContext)
    {
    }
}
