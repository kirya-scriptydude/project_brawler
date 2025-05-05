public partial class BrawlerComponent : Component {

    public Vector3 AnalogMove => Input.AnalogMove;
    public Angles AnalogLook => Input.AnalogLook;

    /// <summary>
    /// AnalogMove that is angled according to main camera.
    /// </summary>
    public Vector3 AnalogMoveAngled = new();
    /// <summary>
    /// Last AnalogMove that is not 0.
    /// </summary>
    public Vector3 MoveDirection = new(1, 0, 0);
    /// <summary>
    /// Last AnalogMove that is not 0. And also angled by camera
    /// </summary>
    public Vector3 MoveDirectionAngled = new(1, 0, 0);

    private void buildInput() {
        if (AnalogMove.Length != 0) {
            MoveDirection = AnalogMove;
            MoveDirectionAngled = AnalogMove * Scene.Camera.LocalRotation;
        } 

        AnalogMoveAngled = AnalogMove * Scene.Camera.LocalRotation;

        Camera.GetComponent<BrawlerCamera>().Rotate(Scene.Camera.LocalRotation * new Vector3(0, AnalogLook.yaw, 0));
    }
}