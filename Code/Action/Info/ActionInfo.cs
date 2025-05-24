/// <summary>
/// Public info library (DamageInfo, HitboxInfo)
/// </summary>
public static class ActionInfo {
    public static DamageInfo GetDamage(AttackType enumName) {
        DamageInfo dmg;
        switch (enumName) {
            default:
                return new DamageInfo(0);

            case AttackType.Fist:
                return new DamageInfo(50, DamageType.Light, DamageSource.Fist);

            case AttackType.FistFinisher:
                dmg = new DamageInfo(250, DamageType.Heavy, DamageSource.Fist);
                dmg.Hitstun = HitstunType.Ground;
                return dmg;

            case AttackType.GenericAttack:
                dmg = new DamageInfo(50, DamageType.Light, DamageSource.Fist);
                return dmg;
        }
    }

    public static HitboxInfo GetHitbox(AttackType enumName) {
        switch (enumName) {
            default:
                return new HitboxInfo(42);

            case AttackType.Fist:
                return new HitboxInfo(32);

            case AttackType.FistFinisher:
                return new HitboxInfo(42);

            case AttackType.GenericAttack:
                return new HitboxInfo(32);
        }
    }
}

