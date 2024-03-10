using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OneBeyond.Studio.Application.SharedKernel.Specifications;

namespace OneBeyond.Studio.DataAccess.EFCore.Repositories;

internal class IncludesTraits<TEntity>
    : IIncludes<TEntity>
    where TEntity : class
{
    public IncludesTraits()
    {
    }

    public IIncludes<TEntity, TChild> Include<TChild>(
        Expression<Func<TEntity, TChild>> navigation)
        where TChild : class
        => new IncludesTraits<TEntity, TChild>();

    public IIncludes<TEntity, TChild> Include<TChild>(
        Expression<Func<TEntity, IEnumerable<TChild>>> navigation)
        where TChild : class
        => new IncludesTraits<TEntity, TChild>();

    public IIncludes<TEntity, TChild> Include<TChild>(
        Expression<Func<TEntity, ICollection<TChild>>> navigation)
        where TChild : class
        => new IncludesTraits<TEntity, TChild>();

    public IIncludes<TEntity, TChild> Include<TChild>(
        Expression<Func<TEntity, IReadOnlyCollection<TChild>>> navigation)
        where TChild : class
        => new IncludesTraits<TEntity, TChild>();
}

internal sealed class IncludesTraits<TEntity, TChild>
    : IncludesTraits<TEntity>
    , IIncludes<TEntity, TChild>
    where TEntity : class
{
    public IncludesTraits()
        : base()
    {
    }

    public IIncludes<TEntity, TNextChild> ThenInclude<TNextChild>(
        Expression<Func<TChild, TNextChild>> navigation)
        where TNextChild : class
        => new IncludesTraits<TEntity, TNextChild>();

    public IIncludes<TEntity, TNextChild> ThenInclude<TNextChild>(
        Expression<Func<TChild, IEnumerable<TNextChild>>> navigation)
        where TNextChild : class
        => new IncludesTraits<TEntity, TNextChild>();

    public IIncludes<TEntity, TNextChild> ThenInclude<TNextChild>(
        Expression<Func<TChild, ICollection<TNextChild>>> navigation)
        where TNextChild : class
        => new IncludesTraits<TEntity, TNextChild>();

    public IIncludes<TEntity, TNextChild> ThenInclude<TNextChild>(
        Expression<Func<TChild, IReadOnlyCollection<TNextChild>>> navigation)
        where TNextChild : class
        => new IncludesTraits<TEntity, TNextChild>();
}
