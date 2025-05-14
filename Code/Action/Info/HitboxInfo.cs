public struct HitboxInfo {
    public float Width, Height;
    public bool MultiHit;

    public HitboxInfo(float width, float height, bool doMultihit = false) {
        Width = width;
        Height = height;
        MultiHit = doMultihit;
    }
}