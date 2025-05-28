using System;

public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "FistFinisher";

    public float Duration { get; set; } = 1.5f;
    public float CancelDuration { get; set; } = 1f;

    public float LastTime { get; set; }

    private Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;
        //todo change magic number
        velocity = Brawler.Object.LocalRotation.Forward * 75;

        Brawler.PerformActionAnimation(AttackType.FistFinisher);
        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        velocity *= 0.95f;
        Brawler.SetVelocity(velocity);

        var time = Time.Now - LastTime;
        var timeFrame = CancelDuration / 3;
        var timeFrame2 = Duration / 3;
        if (time > timeFrame && time < timeFrame2) {
            velocity = Brawler.Object.LocalRotation.Forward * 250;
        }
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}