using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.criteria != null)
        {
            query = query.Where(spec.criteria);//x => x.Brand == brand
        }
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        if (spec.OrderByDesc != null)
        {
            query = query.OrderByDescending(spec.OrderByDesc);
        }
        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }
        return query;
    }
    public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> query,
    ISpecification<T, TResult> spec)
    {
       
        var selectQuery = query as IQueryable<TResult>;
        if (spec.select != null)
        {
            selectQuery = query.Select(spec.select);
        }
        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
}

        return selectQuery ?? query.Cast<TResult>();
    }
}
