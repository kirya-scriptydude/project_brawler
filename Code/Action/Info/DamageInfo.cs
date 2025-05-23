public struct DamageInfo {
    public int Damage;
    public DamageType DamageType;
    public DamageSource DamageSource;

    public bool DoHitstun = true;
    public HitstunType Hitstun = HitstunType.Generic;

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
    Ground
}