public partial class BrawlerComponent : Component {
    [Property, RequireComponent] public CharacterController Controller {get; set;}

    //todo change static variables for stats system
    public static readonly int MOVESPEED = 200;
    public static readonly int GRAVITY_FORCE = -8;

    private void move() {
        var velocity = AnalogMoveAngled.Normal * MOVESPEED;

        if (!Controller.IsOnGround) {
            Controller.Velocity += new Vector3(0, 0, GRAVITY_FORCE);
        } else {
            Controller.Velocity = Controller.Velocity.WithZ(0);
            Controller.ApplyFriction(1.5f);
        }

        //apply rotation
        if (!Input.Down("LockOn")) {
            var pos = GameObject.LocalPosition;
            GameObject.LocalRotation = Rotation.LookAt(Vector3.Direction(pos, pos + MoveDirectionAngled.WithZ(0)));   
        } else {
            //locked on
            velocity /= 2;
        }
        

        Controller.Velocity = velocity.WithZ(Controller.Velocity.z);
        Controller.Move();
    }
}