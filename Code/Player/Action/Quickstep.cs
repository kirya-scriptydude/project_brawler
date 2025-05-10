public class Quickstep : IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;} = "Quickstep";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.2f;
    public float LastTime {get; set;}

    private Vector3 velocity = new();

    public void OnStart() {
        Player.MovementEnabled = false;
        velocity = Player.MoveDirectionAngled * BrawlerComponent.MOVESPEED * 1.5f;
        Player.ModelAnimScale = new Vector3(1, 1, 0.1f);
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;

        Player.Controller.Move();
        Player.ModelAnimScale = new Vector3(1, 1, 0.9f);
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}