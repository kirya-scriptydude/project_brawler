using System.Diagnostics;

/// <summary>
/// Handles object's hurtbox and response to hits. Controls hitstun using animgraph provided by model.
/// </summary>
[Group("Project Brawler")]
public class HurtboxHandler : Component {
    public IBrawler Brawler { get; set; }
    public SkinnedModelRenderer Model { get; set; }

    public HitstunType LastHitstunType { get; set; } = HitstunType.Generic;

    public bool Hitstun { get; private set; } = false;
    public bool Ragdolled { get; private set; } = false;

    public bool IFrame { get; private set; } = false;
    public bool KnockbackDrag { get; private set; } = false;


    /// <summary>
    /// Checks all stun types. Returns true if not affected by stun
    /// </summary>
    public bool NotStunned => !Hitstun && !Ragdolled;

    public HitLog LastHit { get; private set; }

    public static readonly float WALLBOUND_COLLISION_RANGE = 35;

    protected override void OnStart() {
        Brawler = GameObject.GetComponent<IBrawler>();
        Model = Brawler.Model;

        Model.OnAnimTagEvent += tagEvents;
        Model.OnGenericEvent += genericEvents;
    }

    public void Hurt(DamageInfo dmg, GameObject attacker) {
        if (IFrame) return;

        //todo actually deal damage
        if (dmg.DoHitstun) {
            LastHitstunType = dmg.Hitstun;

            Model.Parameters.Set("hitstunType", (int)dmg.Hitstun);
            Model.Parameters.Set("b_hit", true);
            Brawler.ActionHandler.Stop();
        }

        LastHit = new(dmg, attacker);
    }

    private void tagEvents(SceneModel.AnimTagEvent e) {
        if (e.Status == SceneModel.AnimTagStatus.Fired) return;

        switch (e.Name) {
            case "Hitstun":
                Hitstun = e.Status == SceneModel.AnimTagStatus.Start;
                break;
            case "Ragdolled":
                Ragdolled = e.Status == SceneModel.AnimTagStatus.Start;
                break;
            case "IFrame":
                IFrame = e.Status == SceneModel.AnimTagStatus.Start;
                break;
            case "KnockbackDrag":
                KnockbackDrag = e.Status == SceneModel.AnimTagStatus.Start;
                break;
        }



    }

    private void genericEvents(SceneModel.GenericEvent e) { }

    protected override void OnFixedUpdate() {
        if (!NotStunned) {
            var currentVel = Brawler.GetVelocity();
            var knockbackVelocity = HitstunToVelocity(LastHitstunType);

            if (KnockbackDrag) {
                //todo get rid of magic number (get velocity based on knockback type)
                var dir = LastHit.AttackerForward;
                Brawler.SetVelocity(
                    currentVel.LerpTo(dir * knockbackVelocity * LastHit.Damage.KnockbackMultiplier, 0.15f)
                );
                updateWallbound(dir);
            } else {
                Brawler.SetVelocity(currentVel.LerpTo(Vector3.Zero, 0.15f));
            }
        }

    }

    private void updateWallbound(Vector3 dir) {
        if (!HitstunUseWallbound(LastHitstunType)) return;

        var from = WorldPosition + Vector3.Up * 25;
        var to = from + dir * WALLBOUND_COLLISION_RANGE;

        var trace = Scene.Trace
            .Ray(from, to)
            .UsePhysicsWorld()
            .IgnoreGameObjectHierarchy(GameObject)
            .Run();

        if (trace.Hit) {
            var dmg = new DamageInfo(0, DamageType.Generic, DamageSource.Generic);
            dmg.Hitstun = HitstunType.Wallbound;
            Hurt(dmg, GameObject);

            GameObject.LocalRotation = Rotation.LookAt(Vector3.Direction(to, from));
        }
    }

    /// <summary>
    /// why not a lookup dictionary? idk it breaks with enum items apparently 
    /// </summary>
    public float HitstunToVelocity(HitstunType type) => type switch {
        HitstunType.Generic => 50,
        HitstunType.Knockdown => 600,
        HitstunType.Wallbound => -100,
        _ => 0,
    };

    public bool HitstunUseWallbound(HitstunType type) => type switch {
        HitstunType.Knockdown => true,
        HitstunType.Juggle => true,
        _ => false
    };
}

/// <summary>
/// Contains info about latest succesful hit. Stored in HurtboxHandler.
/// </summary>
public class HitLog {

    /// <summary>
    /// When brawler was attacked
    /// </summary>
    public float Timestamp;
    /// <summary>
    /// Where attacker was
    /// </summary>
    public Vector3 AttackerPosition;
    /// <summary>
    /// Where attacker was facing
    /// </summary>
    public Vector3 AttackerForward;
    /// <summary>
    /// How fast was the attacker
    /// </summary>
    public Vector3 AttackerVelocity = new();

    public HitboxInfo Hitbox;
    public DamageInfo Damage;

    public HitLog(DamageInfo dmg, GameObject attacker) {
        Timestamp = Time.Now;
        Damage = dmg;

        AttackerPosition = attacker.WorldPosition;
        AttackerForward = attacker.LocalRotation.Forward;

        var brawler = attacker.GetComponent<IBrawler>();
        if (brawler != null) {
            AttackerVelocity = brawler.GetVelocity();
        }
    }
}

