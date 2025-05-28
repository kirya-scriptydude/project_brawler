public struct DamageInfo {
    public int Damage;
    public DamageType DamageType;
    public DamageSource DamageSource;

    public bool DoHitstun = true;
    public HitstunType Hitstun = HitstunType.Generic;
    public float KnockbackMultiplier = 1f;

    public DamageInfo(int dmg, DamageType type = DamageType.Generic, DamageSource source = DamageSource.Generic) {
        Damage = dmg;
        DamageType = type;
        DamageSource = source;
    }
}

public enum DamageType {
    Generic,
    Light,
    Heavy
}

public enum DamageSource {
    Generic,
    Fist,
    Kick
}

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