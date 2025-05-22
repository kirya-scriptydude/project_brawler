public class Fist : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "Fist";

    public float Duration { get; set; } = 0.8f;
    public float CancelDuration { get; set; } = 0.30f;

    public float HitboxDurationMin { get; set; } = 0.20f;
    public float HitboxDurationMax { get; set; } = 0.35f;

    public float LastTime { get; set; }

    private Vector3 velocity = new();
    private BrawlerComponent Player => Brawler.Object.GetComponent<BrawlerComponent>();
    int combo = 1;

    public static readonly int MAX_COMBO_AMOUNT = 4;

    public void OnStart() {
        Player.MovementEnabled = false;
        //todo change magic number
        velocity = Player.LocalRotation.Forward * 135;

        Brawler.Attack(AttackType.Fist);
        Brawler.Model.Parameters.Set("fist_combo", combo);

        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.92f;
        Player.Controller.Move();
    }

    public void OnStop() {
        Player.MovementEnabled = true;

        if (Time.Now - LastTime < Duration) {
            combo++;
            if (combo > MAX_COMBO_AMOUNT) combo = 1;
        } else combo = 1;
    }
}