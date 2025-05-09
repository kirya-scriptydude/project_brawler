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

    public IReadOnlyDictionary<ActionInputButton, string> ActionToInputName {get;} = new Dictionary<ActionInputButton, string> {
        {ActionInputButton.None, ""},
        {ActionInputButton.Quickstep, "Cross"},
        {ActionInputButton.Fist, "Square"},
        {ActionInputButton.Kick, "Triangle"},
        {ActionInputButton.Grab, "Circle"}
    };

    private void buildInput() {
        if (AnalogMove.Length != 0) {
            MoveDirection = AnalogMove;
            MoveDirectionAngled = AnalogMove * Scene.Camera.LocalRotation;
        } 

        AnalogMoveAngled = AnalogMove * Scene.Camera.LocalRotation;

        //camera :)
        Camera.GetComponent<BrawlerCamera>().Rotate(new Vector3(0, AnalogLook.yaw, 0));
    }

    private void actionControls() {
        foreach (ComboNode node in CurrentComboNode.Children) {
            if (Input.Down(ActionToInputName[node.Button])) {
                ActionActivate(node);
            }
        }
    }

    /// <summary>
    /// Misc controls such as resetting the cam
    /// </summary>
    private void miscControls() {
        if (Input.Pressed("ResetCamera")) {
            BrawlerCamera.ResetPosition();
        }
    }
}