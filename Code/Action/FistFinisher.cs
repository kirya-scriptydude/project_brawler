using System;

public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler { get; set; }
    public string Name { get; } = "FistFinisher";

    public float Duration { get; set; } = 1.9f;
    public float CancelDuration { get; set; } = 1f;

    public float DashTimeframe1 { get; set; } = 0.6f;
    public float DashTimeframe2 { get; set; } = 0.7f;

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
        velocity *= 0.90f;
        Brawler.SetVelocity(velocity);

        var time = Time.Now - LastTime;
        if (time > DashTimeframe1 && time < DashTimeframe2) {
            velocity = Brawler.Object.LocalRotation.Forward * 400;
        }
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}