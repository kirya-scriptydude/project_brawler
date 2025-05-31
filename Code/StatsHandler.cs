/// <summary>
/// Handles and contains stats of a brawler
/// </summary>
[Group("Project Brawler")]
public class StatsHandler : Component {

    [Property, Group("Mobility")] public float MovementSpeed { get; set; } = 400f;
    [Property, Group("Mobility"), Range(0, 2.5f, 0.1f)] public float AttackSpeed { get; set; } = 1f;

    [Property, Group("Health")] public int HP { get; set; } = 1000;
    [Property, Group("Health")] public int MaxHP { get; set; } = 1000;

    [Property, Group("Damage"), Range(0, 5, 0.05f)] public float FistMultiplier { get; set; } = 1f;
    [Property, Group("Damage"), Range(0, 5, 0.05f)] public float KickMultiplier { get; set; } = 1f;

}