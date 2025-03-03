namespace RobustProject.Services.Entities;

public class Entity<TId> : IEntity<TId>
{
    public required TId Id { get; set; }
}