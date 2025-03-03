namespace RobustProject.Services.Models;

public interface IModel<TId>
{
    public TId Id { get; set; }
}