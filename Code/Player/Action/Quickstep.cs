public class Quickstep : IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;} = "Quickstep";

    private float lastTime = 0;
    private Vector3 velocity = new();

    public void OnStart() {
        Player.MovementEnabled = false;
        
        lastTime = Time.Now;
        velocity = Player.MoveDirectionAngled * BrawlerComponent.MOVESPEED * 1.5f;
    }

    public void OnUpdate() {
        //todo replace magic number
        if (Time.Now - lastTime > 0.85) {
            Player.ActionStop();
            return;
        }

        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;

        Player.Controller.Move();
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}