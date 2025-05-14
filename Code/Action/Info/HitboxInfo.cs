public struct HitboxInfo {
    public float Radius;
    public bool MultiHit;

    public Vector3 Offset = new Vector3(20, 0, 40);
    public float Length = 10;

    public HitboxInfo(float radius, bool doMultihit = false) {
       Radius = radius;
        MultiHit = doMultihit;
    }
}