public class Quickstep : IBrawlerAction {
    public IBrawler Brawler {get; set;}
    public string Name {get;} = "Quickstep";

    public float Duration {get; set;} = 0.7f;
    public float CancelDuration {get; set;} = 0.2f;
    public float LastTime {get; set;}

    private float velocity = new();
    private Vector3 wishVelocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;

        velocity = BrawlerComponent.MOVESPEED * 2.5f;
        wishVelocity = Brawler.GetWishVelocity().Normal;
        Brawler.PerformActionAnimation(AttackType.Quickstep);
    }

    public void OnUpdate() {
        var vel = wishVelocity * velocity;

        Brawler.SetVelocity(vel);
        velocity = velocity.LerpTo(0, 0.15f);
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }
}