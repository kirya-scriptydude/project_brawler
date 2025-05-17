using System;

/// <summary>
/// 3rd person camera that follows player
/// </summary>
public sealed class BrawlerCamera : Component {
    [Property, RequireComponent] public CameraComponent Camera {get; set;}
    [Property] public GameObject Subject {get; set;}

    public static readonly int MIN_DISTANCE = 125;
    public static readonly int MAX_DISTANCE = 165;
    public static readonly Vector3 PLR_CAM_HEIGHT = new Vector3(0, -15, 65);

    public GameObject CamPositionObject;
    private GameObject neck;
    
    private Angles rotationVelocity = new();

	protected override void OnStart() {
        neck = Scene.CreateObject();
        neck.LocalPosition = Vector3.Zero;

        CamPositionObject = Scene.CreateObject();
        CamPositionObject.WorldPosition = WorldPosition;
        CamPositionObject.SetParent(neck);
        CamPositionObject.Name = "DefaultCamera";
	}

    protected override void OnUpdate() {
        Camera.FieldOfView = Preferences.FieldOfView;
        neck.WorldPosition = Subject.WorldPosition + Vector3.Up * 35;

        //build rotationVelocity
        var look = Input.AnalogLook;
        rotationVelocity = new Angles(
            Math.Clamp(rotationVelocity.pitch + look.pitch, -5, 5),
            rotationVelocity.yaw + look.yaw,
            0
        );

        neck.LocalRotation = rotationVelocity.ToRotation();

        //raycast from neck to camPosObject. If it hits, camera bumps into smthing
        var trace = Scene.Trace
            .Ray(neck.WorldPosition, CamPositionObject.WorldPosition)
            .IgnoreGameObjectHierarchy(Subject)
            .WithoutTags(["enemy"])
            .UseHitPosition()
            .Run();

        if (trace.Hit) {
            WorldPosition = trace.HitPosition;
            Log.Info(trace.HitPosition);
        } else {
            WorldPosition = WorldPosition.LerpTo(CamPositionObject.WorldPosition, 0.35f);
        }

        //lookat
        LocalRotation = Rotation.LookAt(Vector3.Direction(
            WorldPosition,
            Subject.WorldPosition + PLR_CAM_HEIGHT
        ));

	}

    public void ResetPosition() {
        //WorldPosition = DefaultPositionObject.WorldPosition;
        rotationVelocity = new Angles() * Subject.LocalRotation;
    }
}