using System;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? _criteria)
: ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? criteria => _criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set; }

    public bool IsDistinct { get; set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; set; }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (criteria != null)
        {
            query = query.Where(criteria);
        }
        return query;
    }

    protected void AddOrderby(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDesc = orderByDescExpression;
    }
    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }
    protected void ApplyPagination( int skip,int take)
    {
        Take = take;
        Skip = skip;
        IsPagingEnabled = true;
    }
}
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? _criteria) :
BaseSpecification<T>(_criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, TResult>>? select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>>? selectExpression)
    {
        select = selectExpression;
    }
}