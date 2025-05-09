public class Fist : IBrawlerAction {
    public BrawlerComponent Player {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.30f;

    public float HitboxDurationMin {get; set;} = 0.20f;
    public float HitboxDurationMax = 0.35f;

    public float LastTime {get; set;}

    private Vector3 velocity = new();

    private void handleHitbox() {
        Log.Info("hitbox");
    }

    public void OnStart() {
        Player.MovementEnabled = false;
        //todo change magic number
        velocity = Player.LocalRotation.Forward * 75;
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;
        Player.Controller.Move();

        var time = Time.Now - LastTime;
        if (time > HitboxDurationMin && time < HitboxDurationMax) {
            handleHitbox();
        }
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}