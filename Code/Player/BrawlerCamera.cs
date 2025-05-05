using System;

/// <summary>
/// 3rd person camera that follows player
/// </summary>
public sealed class BrawlerCamera : Component {
    [Property, RequireComponent] public CameraComponent Camera {get; set;}
    [Property] public GameObject Subject {get; set;}

    public static readonly int MIN_DISTANCE = 100;
    public static readonly int MAX_DISTANCE = 175;
    public static readonly Vector3 PLR_CAM_HEIGHT = new Vector3(0, -15, 35);


	protected override void OnUpdate() {
        if (Subject == null) return;

        var from = LocalPosition;
        var to = Subject.LocalPosition + PLR_CAM_HEIGHT;

        //looking at
        LocalRotation = Rotation.LookAt(Vector3.Direction(from, to));

        //positioning
        var push = new Vector3();
        var distance = Vector3.DistanceBetween(from, to);
        if (distance < MIN_DISTANCE) {
            push = (distance - MIN_DISTANCE) * LocalRotation.Forward;
        }
        if (distance > MAX_DISTANCE) {
            push = (distance - MAX_DISTANCE) * LocalRotation.Forward;
        }
        
        LocalPosition += push.WithZ(0);
	}

    public void Rotate(Vector3 axis) {
        LocalPosition += axis;
    }
}