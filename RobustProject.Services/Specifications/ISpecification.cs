using System.Linq.Expressions;

namespace RobustProject.Services.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Predicate { get; }
}