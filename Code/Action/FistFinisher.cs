//todo make hitbox code reusable
using System;

public class FistFinisher : IBrawlerAction {
    public IBrawler Brawler {get; set;}
    public string Name {get;} = "FistFinisher";

    public float Duration {get; set;} = 1.15f;
    public float CancelDuration {get; set;} = 0.8f;

    public float HitboxDurationMin {get; set;} = 0.7f;
    public float HitboxDurationMax {get; set;} = 0.85f;
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
                    comp.ApplyTorque(Random.Shared.VectorInSphere(10) * 4000000);
                    comp.ApplyImpulse((Vector3.Up * 450000) + (Player.LocalRotation.Forward * 200000));
                }
            }
        }
    }

    public void OnStart() {
        Player.MovementEnabled = false;
        velocity = Player.LocalRotation.Forward * 200;
    }

    public void OnUpdate() {
        var time = Time.Now - LastTime;

        if (time > HitboxDurationMin && time < HitboxDurationMax) {
            Player.Controller.Velocity = velocity;
            //Player.ModelAnimScale = new Vector3(1, 1, 0.5f);
            Player.Controller.Move();
            
            handleHitbox();
        }

        
    }

    public void OnStop() {
        Player.MovementEnabled = true;
    }
}