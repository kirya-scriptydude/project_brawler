using System;

public class Fist : IBrawlerAction {
    public IBrawler Brawler {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.30f;

    public float HitboxDurationMin {get; set;} = 0.20f;
    public float HitboxDurationMax {get; set;} = 0.35f;

    public float LastTime {get; set;}

    private Vector3 velocity = new();
    private List<Hitbox> hit = new();
    private BrawlerComponent Player => Brawler.Object.GetComponent<BrawlerComponent>();
    
    private void handleHitbox() {
        var from = Player.WorldPosition + Vector3.Up * 35;
        var to = from + Player.LocalRotation.Forward * 38;
        
        //Gizmo.Draw.LineSphere(from, 24);
        //Gizmo.Draw.LineSphere(to, 24);

        var results = Player.Scene.Trace
            .Sphere(24, from, to)
            .IgnoreGameObjectHierarchy(Player.GameObject)
            .UseHitboxes()
            .WithTag("enemy")
            .RunAll();
        
        foreach (var traceResult in results) {
            if (traceResult.Hitbox == null) continue;

            if (hit.Contains(traceResult.Hitbox)) {
                continue;
            }

            hit.Add(traceResult.Hitbox);

            if (traceResult.GameObject != null) {
                //temporary
                var comp = traceResult.Component.GetComponent<Rigidbody>();
                if (comp != null) {
                    comp.ApplyTorque(Random.Shared.VectorInSphere(10) * 2000000);
                    comp.ApplyImpulse((Vector3.Up * 250000) + (Player.LocalRotation.Forward * 100000));
                    Sound.Play("sounds/hit-test.sound", Player.WorldPosition);
                }
            }
        }
    }

    public void OnStart() {
        Player.MovementEnabled = false;
        //todo change magic number
        velocity = Player.LocalRotation.Forward * 75;

        //Player.ModelAnimScale = new Vector3(2.2f, 1, 1.0f);
    }

    public void OnUpdate() {
        Player.Controller.Velocity = velocity;
        velocity *= 0.91f;
        Player.Controller.Move();

        var time = Time.Now - LastTime;
        if (time > HitboxDurationMin && time < HitboxDurationMax) {
            handleHitbox();
        }
    }

    public void OnStop() {
        hit = new();
        Player.MovementEnabled = true;
    }
}