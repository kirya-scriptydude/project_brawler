using System;

public class Fist : IBrawlerAction {
    public IBrawler Brawler {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.30f;

    public float HitboxDurationMin {get; set;} = 0.20f;
    public float HitboxDurationMax {get; set;} = 0.35f;

    public float LastTime {get; set;}

    private Vector3 velocity = new();
    private BrawlerComponent Player => Brawler.Object.GetComponent<BrawlerComponent>();

    public void OnStart() {
        Player.MovementEnabled = false;
        //todo change magic number
        velocity = Player.LocalRotation.Forward * 75;
        
        Brawler.MoveInfoEntry = InfoEntry.FistLight;
        Brawler.Attack(AnimgraphAttackType.Fist);

        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;
        Player.Controller.Move();

        //var time = Time.Now - LastTime;
        //if (time > HitboxDurationMin && time < HitboxDurationMax) {
        //    handleHitbox();
        //}
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}