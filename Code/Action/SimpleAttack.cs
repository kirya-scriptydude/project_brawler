/// <summary>
/// generic attack class for most of the needed strikes.
/// </summary>
public class SimpleAttack : IBrawlerAction {
    public IBrawler Brawler { get; set; }

    public string Name => "SimpleAttack";

    public float Duration { get; set; } = 1f;
    public float CancelDuration { get; set; } = 1f;
    public float LastTime { get; set; }

    public void OnStart() {
        Brawler.MovementEnabled = false;
        Brawler.PerformActionAnimation(AttackType.GenericAttack);
    }

    public void OnUpdate() {
        
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }

    
}