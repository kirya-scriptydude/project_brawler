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