using oop_s2_2_mvc_71757.Models;

namespace oop_s2_2_mvc_71757.ViewModels;

public class DashboardViewModel
{
    public int InspectionsThisMonth { get; set; }
    public int FailedInspectionsThisMonth { get; set; }
    public int OverdueOpenFollowUps { get; set; }

    public string? SelectedTown { get; set; }
    public RiskRating? SelectedRiskRating { get; set; }

    public IReadOnlyList<string> Towns { get; set; } = Array.Empty<string>();
}
