namespace RobustProject.Services.Entities;

public class Rosebud : Entity<int?>
{
    public required string Family { get; set; }
    public required int Amount { get; set; }
}