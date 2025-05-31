using System;

/// <summary>
/// Handles object's hurtbox and response to hits. Controls hitstun using animgraph provided by model.
/// </summary>
[Group("Project Brawler")]
public class HurtboxHandler : Component {
    public IBrawler Brawler { get; set; }
    public SkinnedModelRenderer Model { get; set; }

    public HitstunType LastHitstunType { get; set; } = HitstunType.Generic;

    [Property, ReadOnly, Group("Tags")] public bool Hitstun { get; private set; } = false;
    [Property, ReadOnly, Group("Tags")] public bool Ragdolled { get; private set; } = false;
    [Property, ReadOnly, Group("Tags")] public bool IFrame { get; private set; } = false;
    [Property, ReadOnly, Group("Tags")] public bool KnockbackDrag { get; private set; } = false;
    [Property, ReadOnly, Group("Tags")] public bool Blocking { get; private set; }

    /// <summary>
    /// After hitting a block you enter slight blockstun. While blockstunned you cannot stop blocking. True if block stun active.
    /// </summary>
    public bool IsBlockstunned { get {
            return Time.Now - LastBlockHit < BLOCKSTUN_SECONDS;   
        }
    }
    public float LastBlockHit { get; private set; }

    public static readonly float BLOCKSTUN_SECONDS = 0.35f;


    /// <summary>
    /// Checks all stun types. Returns true if not affected by stun
    /// </summary>
    public bool NotStunned => !Hitstun && !Ragdolled;

    public HitLog LastHit { get; private set; }

    public static readonly float WALLBOUND_COLLISION_RANGE = 35;

    /// <summary>
    /// How much brawler was hit without leaving hitstun.
    /// </summary>
    public int ConsecutiveHits { get; private set; }

    protected override void OnStart() {
        Brawler = GameObject.GetComponent<IBrawler>();
        Model = Brawler.Model;

        Model.OnAnimTagEvent += tagEvents;
        Model.OnGenericEvent += genericEvents;
    }

    /// <summary>
    /// Try to hurt brawler with provided DamageInfo. 
    /// </summary>
    /// <returns>Has brawler been successfully hit?</returns>
    public bool Hurt(DamageInfo dmg, GameObject attacker) {
        if (IFrame) return false;

        if (NotStunned && dmg.DoHitstun)
                ConsecutiveHits = 0;
            else
                ConsecutiveHits++;

        if (Blocking) {
            hurtBlock(dmg, attacker);
            return true;
        }

        var reaction = reactionNodes();
        if (!reaction) return false;

        //todo actually deal damage
        if (dmg.DoHitstun) {
            var chosenHitstun = Hitstun ? chooseHitstun(dmg.Hitstun) : dmg.Hitstun;
            LastHitstunType = chosenHitstun;

            Model.Parameters.Set("hitstunType", (int)chosenHitstun);
            Model.Parameters.Set("b_hit", true);
            Brawler.ActionHandler.Stop();
        }

        if (dmg.PlayHitSound) {
            Sound.Play(dmg.HitSound, WorldPosition);
        }

        LastHit = new(dmg, attacker);

        return true;
    }

    /// <summary>
    /// handle block stuff. Blockstun and blockbreak 
    /// </summary>
    private void hurtBlock(DamageInfo dmg, GameObject attacker) {
        if (dmg.DoHitstun) {
            Model.Parameters.Set("b_blockHit", true);
            LastBlockHit = Time.Now;

            LocalRotation = Rotation.LookAt(Vector3.Direction(WorldPosition, attacker.WorldPosition.WithZ(WorldPosition.z)));
        }

        if (dmg.PlayHitSound) Sound.Play("sounds/blockhit.sound", WorldPosition);
    }

    /// <summary>
    /// Handle reaction nodes such as blocking and dashing. If returns false, then it used a reaction move.
    /// </summary>
    private bool reactionNodes() {
        if (Brawler is EnemyComponent) {
            var action = Brawler.ActionHandler;

            foreach (var node in action.CurrentNode.Children) {
                if (!NotStunned) {
                    if (!HitstunHelper.IsWeakHitstun(LastHitstunType)) continue;
                }

                if (node.ReactionCondition((EnemyComponent)Brawler)) {
                    action.Use(node);
                    return false;
                }
            }

        }

        return true;
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
            case "Block":
                Blocking = e.Status == SceneModel.AnimTagStatus.Start;
                break;
        }



    }

    private void genericEvents(SceneModel.GenericEvent e) { }

    protected override void OnFixedUpdate() {
        if (!NotStunned) {
            var currentVel = Brawler.GetVelocity();
            var knockbackVelocity = HitstunHelper.GetKnockbackVelocity(LastHitstunType);

            if (KnockbackDrag) {
                var dir = LastHit.AttackerForward;
                updateWallbound(dir);
                //todo get rid of magic number (get velocity based on knockback type)
                
                Brawler.SetVelocity(
                    currentVel.LerpTo(dir * knockbackVelocity * LastHit.Damage.KnockbackMultiplier, 0.15f)
                );
                
            } else {
                Brawler.SetVelocity(currentVel.LerpTo(Vector3.Zero, 0.15f));
            }
        }

    }

    public static readonly float WALLBOUND_TIME = 1f;
    private float lastWallbound = Time.Now;
    private float wallboundScale = 1f;
    private void updateWallbound(Vector3 dir) {
        if (!HitstunHelper.CanUseWallbound(LastHitstunType)) return;

        var from = WorldPosition + Vector3.Up * 25;
        var to = from + dir * WALLBOUND_COLLISION_RANGE;

        var trace = Scene.Trace
            .Sphere(16f, from, to)
            .UsePhysicsWorld()
            .IgnoreGameObjectHierarchy(GameObject)
            .Run();

        if (trace.Hit) {
            //speed up wallbounds if already hit a good amount of them.
            if (Time.Now - lastWallbound < WALLBOUND_TIME) {
                wallboundScale = Math.Clamp(wallboundScale + 0.15f, 1f, 2.5f);
            } else wallboundScale = 1;

            Log.Info(Time.Now - lastWallbound);

            Model.Parameters.Set("wallboundTimeScale", wallboundScale);

            var dmg = new DamageInfo(0, DamageType.Generic, DamageSource.Generic);
            dmg.Hitstun = HitstunType.Wallbound;
            Hurt(dmg, GameObject);

            GameObject.LocalRotation = Rotation.LookAt(Vector3.Direction(to, from));
            
            lastWallbound = Time.Now;
            
        }

        
    }

    private HitstunType chooseHitstun(HitstunType newHit) {
        var old = LastHitstunType;
        var overriden = HitstunHelper.HitstunOverride(old);

        return HitstunHelper.GetWeight(newHit) > HitstunHelper.GetWeight(overriden) ? newHit : overriden;
    }
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

