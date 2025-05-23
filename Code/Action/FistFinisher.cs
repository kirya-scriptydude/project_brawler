using System;

public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "FistFinisher";

    public float Duration { get; set; } = 1.35f;
    public float CancelDuration { get; set; } = 1f;

    public float LastTime { get; set; }

    private Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;
        //todo change magic number
        velocity = Brawler.Object.LocalRotation.Forward * 75;

        Brawler.Attack(AttackType.FistFinisher);
        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        velocity *= 0.95f;
        Brawler.SetVelocity(velocity);

        var time = Time.Now - LastTime;
        var timeFrame = Duration / 3;
        if (time > timeFrame && time < CancelDuration / 2) {
            velocity = Brawler.Object.LocalRotation.Forward * 200;
        }
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}