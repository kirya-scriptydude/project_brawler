using System;

public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler {get; set;} 
    public string Name {get;} = "FistFinisher";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.30f;

    public float HitboxDurationMin {get; set;} = 0.20f;
    public float HitboxDurationMax {get; set;} = 0.35f;

    public float LastTime {get; set;}

    private Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;
        //todo change magic number
        velocity = Brawler.Object.LocalRotation.Forward * 75;
        
        Brawler.Attack(AttackType.FistFinisher);
        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        velocity *= 0.91f;
        Brawler.SetVelocity(velocity);

        //var time = Time.Now - LastTime;
        //if (time > HitboxDurationMin && time < HitboxDurationMax) {
        //    handleHitbox();
        //}
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}