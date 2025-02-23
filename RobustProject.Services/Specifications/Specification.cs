using System.Linq.Expressions;

namespace RobustProject.Services.Specifications;

public class Specification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Predicate { get; }

    protected Specification(Expression<Func<T, bool>> predicate)
    {
        Predicate = predicate;
    }
}