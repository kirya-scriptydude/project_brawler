public enum HitstunType {
    /// <summary>
    /// A lil flinch. Most common type
    /// </summary>
    Generic,
    /// <summary>
    /// Go straight into ragdoll state without any anims. Not recommended to use other than testing.
    /// </summary>
    Ground,
    /// <summary>
    /// You fly forward and land into ragdoll state.
    /// </summary>
    Knockdown,
    /// <summary>
    /// You get stunned and stay in place for a good amount of time. Not used manually, but rather when hitting the walls.
    /// </summary>
    Wallbound,
    /// <summary>
    /// You swiftly fall to the ground. This is the reaction of most knockdowns when another move landed.
    /// </summary>
    Juggle
}

/// <summary>
/// static class with a bunch of switch statements that tells info about HitstunTypes
/// </summary>
public static class HitstunHelper {

    /// <summary>
    /// Get velocity that is applied when KnockbackDrag is active. 
    /// </summary>
    public static float GetKnockbackVelocity(HitstunType type) => type switch {
        HitstunType.Generic => 50,
        HitstunType.Knockdown => 600,
        HitstunType.Wallbound => -100,
        HitstunType.Juggle => 200,

        _ => 0,
    };

    /// <summary>
    /// Can my hitstun trigger wallbound?
    /// </summary>
    public static bool CanUseWallbound(HitstunType type) => type switch {
        HitstunType.Knockdown => true,
        HitstunType.Juggle => true,

        _ => false
    };

    /// <summary>
    /// Hitstun's weight that used to determine which to use on overrides. (check HitstunOverride)
    /// </summary>
    /// <returns>weight</returns>
    public static int GetWeight(HitstunType type) => type switch {
        HitstunType.Knockdown => 100,
        HitstunType.Juggle => 49,
        HitstunType.Wallbound => 50,

        _ => 0
    };

    /// <summary>
    /// Choose a different hitstun depending on already active one. For example when you hitstun opponent with Knockdown, your hitstun is forced to be Juggle. Weight from GetWeight still applies.
    /// </summary>
    /// <param name="type">Active hitstun</param>
    /// <returns>Chosen hitstun override</returns>
    public static HitstunType HitstunOverride(HitstunType type) => type switch {
        HitstunType.Knockdown => HitstunType.Juggle,
        HitstunType.Wallbound => HitstunType.Juggle,

        _ => type
    };

    public static bool IsWeakHitstun(HitstunType type) => type switch {
        HitstunType.Generic => true,

        _ => false
    };
}