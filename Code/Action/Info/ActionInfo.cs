/// <summary>
/// Public info library (DamageInfo, HitboxInfo)
/// </summary>
public static class ActionInfo {
    public static DamageInfo GetDamage(InfoEntry enumName) {
        switch(enumName) {
            default:
                return new DamageInfo(0);

            case InfoEntry.FistLight: 
                return new DamageInfo(50, DamageType.Light, DamageSource.Fist);
        }
    }

    public static HitboxInfo GetHitbox(InfoEntry enumName) {
        switch(enumName) {
            default:
                return new HitboxInfo(24);

            case InfoEntry.FistLight:
                return new HitboxInfo(16);
        }
    }
}

public enum InfoEntry {
        Default,
        FistLight
    }

