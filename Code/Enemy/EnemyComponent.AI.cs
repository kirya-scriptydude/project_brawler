public partial class EnemyComponent : Component, IBrawler {
    /// <summary>
    /// How much more likely an AI to attack.
    /// </summary>
    [Property, Group("AI Settings"), Range(0, 20, 1)] public int Aggression {get; set;} = 1;

    /// <summary>
    /// How much AI more likely to dodge attacks and generally use quickstep.
    /// </summary>
    [Property, Group("AI Settings"), Range(0, 20, 1)] public int Evasion {get; set;} = 1;

    /// <summary>
    /// Randomly quickstep just cause. Depends on evasion levels.
    /// </summary>
    [Property, Group("AI Settings")] public bool QuickstepInNeutral {get; set;} = false;

    /// <summary>
    /// Number of chained quicksteps that AI can perform. By default - 0 (none)
    /// </summary>
    [Property, Group("AI Settings")] public int ChainQuickstep {get; set;} = 0;
}