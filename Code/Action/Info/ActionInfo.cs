/// <summary>
/// Public info library (DamageInfo, HitboxInfo)
/// </summary>
public static class ActionInfo {
    public static DamageInfo GetDamage(InfoEntry enumName) {
        switch(enumName) {
            default:
                return new DamageInfo(0);
        }
    }

    public static HitboxInfo GetHitbox(InfoEntry enumName) {
        switch(enumName) {
            default:
                return new HitboxInfo(24);
        }
    }

    public enum InfoEntry {
        Default
    }
}

