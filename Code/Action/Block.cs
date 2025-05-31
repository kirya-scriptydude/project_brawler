public class BlockPlayer : IBrawlerAction {
    public IBrawler Brawler { get; set; }

    public string Name => "BlockPlayer";

    public float Duration { get; set; } = -1;
    public float CancelDuration { get; set; } = 0;
    public float LastTime { get; set; }

    Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;
        Brawler.PerformActionAnimation(AttackType.Block);
    }

    public void OnUpdate() {
        var hit = Brawler.HurtboxHandler.IsBlockstunned;
        if (!Input.Down("Block") && !hit) Brawler.ActionHandler.Stop();

        Brawler.SetVelocity(velocity);

        if (hit) {
            velocity = Vector3.Backward * 35 * Brawler.Object.LocalRotation;
        } else {
            velocity = new();
        }

    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
        Brawler.Model.Parameters.Set("b_finishAction", true);
    }
}


public class BlockNPC : IBrawlerAction {
    public IBrawler Brawler { get; set; }

    public string Name => "BlockNPC";

    public float Duration { get; set; } = -1;
    public float CancelDuration { get; set; } = 0;
    public float LastTime { get; set; }

    static readonly float UNBLOCK_TIME = 1.65f;

    public void OnStart() {
        Brawler.MovementEnabled = false;
        Brawler.PerformActionAnimation(AttackType.Block);
    }

    public void OnUpdate() {
        if (Time.Now - LastTime < UNBLOCK_TIME) return;
        var blockHitTime = Time.Now - Brawler.HurtboxHandler.LastBlockHit;

        if (blockHitTime > UNBLOCK_TIME)
            Brawler.ActionHandler.Stop();
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
        Brawler.Model.Parameters.Set("b_finishAction", true);
    }
}