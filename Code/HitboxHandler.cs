/// <summary>
/// Handles hitbox based on set damage and hitbox info.
/// </summary>
[Group("Project Brawler")]
public class HitboxHandler : Component {
    [Property, ReadOnly] public bool HitboxActive {get; set;}

    private List<Hitbox> hit = new();

    /// <summary>
    /// Activate hitbox. If end frame is true, reset the hit list after that iteration.
    /// </summary>
    public void Cast(DamageInfo dmg, HitboxInfo hitbox, bool endFrame = false) {
        var pos = WorldPosition + (hitbox.Offset * LocalRotation);
        var ray = new Ray(pos, LocalRotation.Forward);

        var traces = Scene.Trace
            .Sphere(hitbox.Radius, ray, hitbox.Length)
            .WithTag("enemy")
            .IgnoreGameObjectHierarchy(GameObject)
            .UseHitboxes()
            .RunAll();
        
        
        if (Game.IsEditor) {
            DebugOverlay.Sphere(new Sphere(ray.Position, hitbox.Radius), default, 0.5f);
            DebugOverlay.Sphere(new Sphere(ray.Position + ray.Forward * hitbox.Length, hitbox.Radius), Color.Red, 0.5f);
        }

        foreach (var traceResult in traces) {
            if (!traceResult.Hit) return;
            if (traceResult.Hitbox == null) return;
            if (traceResult.Hitbox.GameObject == null) return;
            if (hit.Contains(traceResult.Hitbox)) return;

            if (!hitbox.MultiHit) hit.Add(traceResult.Hitbox);
            tryDamage(dmg);
        }

        if (endFrame) {
            hit = new();
        }
    }

    //todo damage stun etc etc all that number stuff
    private void tryDamage(DamageInfo dmg) {

    }
}