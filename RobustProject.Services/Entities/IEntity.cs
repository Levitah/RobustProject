namespace RobustProject.Services.Entities;

public interface IEntity<TId>
{
    TId Id { get; set; }
}