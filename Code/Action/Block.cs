public class BlockPlayer : IBrawlerAction {
	public IBrawler Brawler { get; set; }

    public string Name => "BlockPlayer";

    public float Duration { get; set; } = -1;
    public float CancelDuration { get; set; } = 0;
    public float LastTime { get; set; }

    public void OnStart() {
        Brawler.MovementEnabled = false;
    }

    public void OnUpdate() {
        if (!Input.Down("Block")) Brawler.ActionHandler.Stop();
	}

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}