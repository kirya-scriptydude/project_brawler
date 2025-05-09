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
            var newRot = Rotation.LookAt(Vector3.Direction(pos, pos + MoveDirectionAngled.WithZ(0))); 
            LocalRotation = LocalRotation.LerpTo(newRot, 0.3f);
        } else {
            //locked on
            velocity /= 2;

            if (LockOnTarget != null) {
                var pos = GameObject.WorldPosition;
                var newRot = Rotation.LookAt(Vector3.Direction(pos, LockOnTarget.WorldPosition.WithZ(pos.z)));
                LocalRotation = LocalRotation.LerpTo(newRot, 0.5f);
            } 
            
        }
        

        Controller.Velocity = Controller.Velocity.LerpTo(velocity, 0.35f).WithZ(Controller.Velocity.z);
        Controller.Move();
    }
}