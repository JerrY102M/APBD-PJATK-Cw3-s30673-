using System.ComponentModel.DataAnnotations;

namespace TrainingCenterApi.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Id sali musi być większe od zera.")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Nazwa organizatora jest wymagana.")]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temat rezerwacji jest wymagany.")]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [Required(ErrorMessage = "Status jest wymagany.")]
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(OrganizerName))
        {
            yield return new ValidationResult(
                "Nazwa organizatora nie może być pusta.",
                new[] { nameof(OrganizerName) });
        }

        if (string.IsNullOrWhiteSpace(Topic))
        {
            yield return new ValidationResult(
                "Temat rezerwacji nie może być pusty.",
                new[] { nameof(Topic) });
        }

        if (string.IsNullOrWhiteSpace(Status))
        {
            yield return new ValidationResult(
                "Status nie może być pusty.",
                new[] { nameof(Status) });
        }

        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia.",
                new[] { nameof(EndTime), nameof(StartTime) });
        }
    }
}
