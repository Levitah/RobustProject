using RobustProject.Services.Validation;
using System.ComponentModel.DataAnnotations;

namespace RobustProject.Services.Models;

public class RosebudModel : IModel<int?>, IValidatable
{
    public int? Id { get; set; }

    [Required]
    public required string Family { get; set; }

    [Required]
    public required int Amount { get; set; }
}