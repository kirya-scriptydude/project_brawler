public class Quickstep : IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;} = "Quickstep";

    public float Duration {get; set;} = 0.8f;
    public float LastTime {get; set;}

    private Vector3 velocity = new();

    public void OnStart() {
        Player.MovementEnabled = false;
        velocity = Player.MoveDirectionAngled * BrawlerComponent.MOVESPEED * 1.5f;
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;

        Player.Controller.Move();
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}