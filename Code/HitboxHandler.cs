/// <summary>
/// Handles hitbox based on set damage and hitbox info.
/// </summary>
[Group("Project Brawler")]
public class HitboxHandler : Component {
    [Property, ReadOnly] public bool HitboxActive { get; set; }

    private List<Hitbox> hit = new();
    private int durationLeft = 0;

    private DamageInfo damageInfo;
    private HitboxInfo hitboxInfo;

    /// <summary>
    /// Activate hitbox. If end frame is true, reset the hit list after that iteration.
    /// </summary>
    public void Cast(DamageInfo dmg, HitboxInfo hitbox, int lengthFrames) {
        damageInfo = dmg;
        hitboxInfo = hitbox;
        durationLeft = lengthFrames;
    }

    private void tryDamage(DamageInfo dmg, Hitbox hitbox) {
        var brawler = hitbox.GameObject.Components.Get<IBrawler>();
        if (brawler == null) return;

        brawler.HurtboxHandler.Hurt(dmg, GameObject);
    }

    protected override void OnFixedUpdate() {
        if (durationLeft <= 0) return;
        

        var pos = WorldPosition + (hitboxInfo.Offset * LocalRotation);
        var ray = new Ray(pos, LocalRotation.Forward);

        var traces = Scene.Trace
            .Sphere(hitboxInfo.Radius, ray, hitboxInfo.Length)
            .WithTag("brawler")
            .IgnoreGameObjectHierarchy(GameObject)
            .UseHitboxes()
            .RunAll();


        //if (Game.IsEditor) {
        DebugOverlay.Sphere(new Sphere(ray.Position, hitboxInfo.Radius), default, 0.5f);
        DebugOverlay.Sphere(new Sphere(ray.Position + ray.Forward * hitboxInfo.Length, hitboxInfo.Radius), Color.Red, 0.5f);
        //}

        foreach (var traceResult in traces) {
            if (!traceResult.Hit) continue;
            if (traceResult.Hitbox == null) continue;
            if (traceResult.Hitbox.GameObject == null) continue;
            if (hit.Contains(traceResult.Hitbox)) continue;

            if (!hitboxInfo.MultiHit) hit.Add(traceResult.Hitbox);
            tryDamage(damageInfo, traceResult.Hitbox);

            hit.Add(traceResult.Hitbox);
        }

        durationLeft--;
        if (durationLeft <= 0) {
            hit = new();
        }
	}
}