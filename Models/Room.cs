using System.ComponentModel.DataAnnotations;

namespace TrainingCenterApi.Models;

public class Room : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod budynku jest wymagany.")]
    public string BuildingCode { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Pojemność sali musi być większa od zera.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult(
                "Nazwa sali nie może być pusta.",
                new[] { nameof(Name) });
        }

        if (string.IsNullOrWhiteSpace(BuildingCode))
        {
            yield return new ValidationResult(
                "Kod budynku nie może być pusty.",
                new[] { nameof(BuildingCode) });
        }
    }
}
