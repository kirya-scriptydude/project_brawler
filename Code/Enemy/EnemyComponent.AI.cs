public partial class EnemyComponent : Component, IBrawler {
    /// <summary>
    /// How much more likely an AI to attack.
    /// </summary>
    [Property, Group("AI Settings"), Range(0, 20, 1)] public int Aggression { get; set; } = 1;

    /// <summary>
    /// How much AI more likely to dodge attacks and generally use quickstep.
    /// </summary>
    [Property, Group("AI Settings"), Range(0, 20, 1)] public int Evasion { get; set; } = 1;
    
    /// <summary>
    /// How much AI more likely to block and use defensive moves.
    /// </summary>
    [Property, Group("AI Settings"), Range(0, 20, 1)] public int Defense {get; set;} = 1;

}