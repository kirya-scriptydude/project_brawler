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
            .WithTag("brawler")
            .IgnoreGameObjectHierarchy(GameObject)
            .UseHitboxes()
            .RunAll();
        
        
        //if (Game.IsEditor) {
            DebugOverlay.Sphere(new Sphere(ray.Position, hitbox.Radius), default, 0.5f);
            DebugOverlay.Sphere(new Sphere(ray.Position + ray.Forward * hitbox.Length, hitbox.Radius), Color.Red, 0.5f);
        //}

        foreach (var traceResult in traces) {
            if (!traceResult.Hit) continue;
            if (traceResult.Hitbox == null) continue;
            if (traceResult.Hitbox.GameObject == null) continue;
            if (hit.Contains(traceResult.Hitbox)) continue;

            if (!hitbox.MultiHit) hit.Add(traceResult.Hitbox);
            tryDamage(dmg, traceResult.Hitbox);

            hit.Add(traceResult.Hitbox);
        }

        if (endFrame) {
            hit = new();
        }
    }

    //todo damage stun etc etc all that number stuff
    private void tryDamage(DamageInfo dmg, Hitbox hitbox) {
        var brawler = hitbox.GameObject.Components.Get<IBrawler>();
        if(brawler == null) return;

        brawler.Health.Inflict(dmg.Damage);
    }
}