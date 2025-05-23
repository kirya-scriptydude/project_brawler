/// <summary>
/// Public info library (DamageInfo, HitboxInfo)
/// </summary>
public static class ActionInfo {
    public static DamageInfo GetDamage(AttackType enumName) {
        switch (enumName) {
            default:
                return new DamageInfo(0);

            case AttackType.Fist:
                return new DamageInfo(50, DamageType.Light, DamageSource.Fist);
            
            case AttackType.FistFinisher:
                var dmg = new DamageInfo(250, DamageType.Heavy, DamageSource.Fist);
                dmg.Hitstun = HitstunType.Ground;
                return dmg;
        }
    }

    public static HitboxInfo GetHitbox(AttackType enumName) {
        switch (enumName) {
            default:
                return new HitboxInfo(24);

            case AttackType.Fist:
                return new HitboxInfo(16);
                
            case AttackType.FistFinisher:
                return new HitboxInfo(24);
        }
    }
}

