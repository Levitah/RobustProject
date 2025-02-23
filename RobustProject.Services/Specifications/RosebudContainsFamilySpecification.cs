namespace RobustProject.Services.Specifications;

public class RosebudContainsFamilySpecification : Specification<Entities.Rosebud>
{
    public RosebudContainsFamilySpecification(string family) : base(x => x.Family.Equals(family, StringComparison.InvariantCultureIgnoreCase))
    {

    }
}