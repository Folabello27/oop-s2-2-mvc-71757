using System.ComponentModel.DataAnnotations;

namespace oop_s2_2_mvc_71757.Models;

public class FollowUp : IValidatableObject
{
    public int Id { get; set; }

    [Required]
    public int InspectionId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    [Required]
    public FollowUpStatus Status { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ClosedDate { get; set; }

    public Inspection? Inspection { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Status == FollowUpStatus.Closed && ClosedDate is null)
        {
            yield return new ValidationResult(
                "Closed follow-ups must have a closed date.",
                new[] { nameof(ClosedDate) });
        }

        if (Status == FollowUpStatus.Open && ClosedDate is not null)
        {
            yield return new ValidationResult(
                "Open follow-ups cannot have a closed date.",
                new[] { nameof(ClosedDate) });
        }
    }
}
