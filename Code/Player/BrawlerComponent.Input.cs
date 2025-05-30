using System;

public partial class BrawlerComponent : Component {

    public Vector3 AnalogMove { get; set; } = new();
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

    public GameObject LockOnTarget {get; set;}

    public string ActionInputToName(ActionInputButton button) => button switch {
        ActionInputButton.Quickstep => "Cross",
        ActionInputButton.Fist => "Square",
        ActionInputButton.Kick => "Triangle",
        ActionInputButton.Grab => "Circle",
        ActionInputButton.Block => "Block",

        _ => ""
    };

    private void buildInput() {
        AnalogMove = Input.AnalogMove;
        if (AnalogMove.Length > 1) AnalogMove = AnalogMove.ClampLength(1);

        if (AnalogMove.Length != 0) {
            MoveDirection = AnalogMove;
            MoveDirectionAngled = AnalogMove * Scene.Camera.LocalRotation;
        }

        AnalogMoveAngled = AnalogMove * Scene.Camera.LocalRotation;
    }

    private void actionControls() {
        if (!HurtboxHandler.NotStunned) return;
        
        foreach (ComboNode node in ActionHandler.CurrentNode.Children) {
                if (Input.Down(ActionInputToName(node.Button))) {
                    ActionHandler.Use(node);
                }
            }
    }

    /// <summary>
    /// Misc controls such as resetting the cam, locking on
    /// </summary>
    private void miscControls() {
        if (Input.Pressed("ResetCamera")) {
            BrawlerCamera.ResetPosition();
        }

        if (Input.Pressed("LockOn")) {
            var enemies = new SortedList<float, GameObject>();

            foreach (var obj in Scene.GetAllObjects(true)) {
                if (!obj.IsPrefabInstanceRoot) continue;
                if (obj.Tags.Has("enemy")) {
                    var distance = Vector3.DistanceBetween(obj.WorldPosition, WorldPosition);
                    enemies.Add(distance, obj);
                }
            }

            if (enemies.Count > 0) {
                //pick closest enemy around
                var closest = enemies.FirstOrDefault().Value;
                LockOnTarget = closest;
            }
        } else if (Input.Released("LockOn")) {
            MoveDirectionAngled = LocalRotation.Forward;
        }
    }
}