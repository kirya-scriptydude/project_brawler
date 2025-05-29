public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "FistFinisher";

    public float Duration { get; set; } = 1.9f;
    public float CancelDuration { get; set; } = 1f;

    public float DashTimeframe1 { get; set; } = 0.6f;
    public float DashTimeframe2 { get; set; } = 0.7f;

    public float LastTime { get; set; }

    Vector3 velocity = new();
    bool reachedHitbox = false;

    public void OnStart() {
        Brawler.MovementEnabled = false;

        Brawler.PerformActionAnimation(AttackType.FistFinisher);

        velocity = Brawler.ActionHandler.CurrentNode.HitboxInfo.DashVelocity * Brawler.Object.LocalRotation;
        reachedHitbox = false;
    }

    public void OnUpdate() {
        if (Brawler.HitboxHandler.HitboxActive) reachedHitbox = true;
        if (!reachedHitbox) return;
        
        Brawler.SetVelocity(velocity);
        velocity = velocity.LerpTo(Vector3.Zero, 0.15f);
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}