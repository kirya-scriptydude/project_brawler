public class BlockPlayer : IBrawlerAction {
	public IBrawler Brawler { get; set; }

    public string Name => "BlockPlayer";

    public float Duration { get; set; } = -1;
    public float CancelDuration { get; set; } = 0;
    public float LastTime { get; set; }

    public void OnStart() {
        Brawler.MovementEnabled = false;
        Brawler.PerformActionAnimation(AttackType.Block);
    }

    public void OnUpdate() {
        if (!Input.Down("Block") && !Brawler.HurtboxHandler.IsBlockstunned) Brawler.ActionHandler.Stop();
	}

    public void OnStop() {
        Brawler.MovementEnabled = true;
        Brawler.Model.Parameters.Set("b_finishAction", true);
    }
}