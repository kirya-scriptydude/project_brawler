public class Fist : IBrawlerAction {
    public BrawlerComponent Player {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 1;
    public float CancelDuration {get; set;} = 0.35f;
    public float LastTime {get; set;}

    private Vector3 velocity = new();

    public void OnStart() {
        Player.MovementEnabled = false;
        //todo change magic number
        velocity = Player.LocalRotation.Forward * 75;
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