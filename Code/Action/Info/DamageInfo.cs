public struct DamageInfo {
    public int Damage;
    public DamageType DamageType;
    public DamageSource DamageSource;

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