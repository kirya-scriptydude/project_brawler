public class Fist : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "Fist";

    public float Duration { get; set; } = 0.8f;
    public float CancelDuration { get; set; } = 0.30f;

    public float HitboxDurationMin { get; set; } = 0.20f;
    public float HitboxDurationMax { get; set; } = 0.35f;

    public float LastTime { get; set; }

    private BrawlerComponent Player => Brawler.Object.GetComponent<BrawlerComponent>();

    public static readonly int MAX_COMBO_AMOUNT = 4;

    Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;

        if (Player != null) {
            Brawler.Model.Parameters.Set("fist_combo", Player.ActionHandler.CurrentNode.TreeLevel);
        }

        Brawler.PerformActionAnimation(AttackType.Fist);
        
        velocity = Brawler.ActionHandler.CurrentNode.HitboxInfo.DashVelocity * Brawler.Object.LocalRotation;
    }

    public void OnUpdate() {
        Brawler.SetVelocity(velocity);
        velocity = velocity.LerpTo(Vector3.Zero, 0.15f);
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}