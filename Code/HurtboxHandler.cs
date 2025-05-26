/// <summary>
/// Handles object's hurtbox and response to hits. Controls hitstun using animgraph provided by model.
/// </summary>
[Group("Project Brawler")]
public class HurtboxHandler : Component {
    public IBrawler Brawler { get; set; }
    public SkinnedModelRenderer Model { get; set; }

    public HitstunType LastType { get; set; } = HitstunType.Generic;

    public bool Hitstun { get; private set; } = false;
    public bool Ragdolled { get; private set; } = false;

    public bool IFrame { get; private set; } = false;
    public bool KnockbackDrag { get; private set; } = false;


    /// <summary>
    /// Checks all stun types. Returns true if not affected by stun
    /// </summary>
    public bool NotStunned => !Hitstun && !Ragdolled;

    public HitLog LastHit { get; private set; }


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
        if (KnockbackDrag) {
            //todo get rid of magic number (get velocity based on knockback)
            Brawler.SetVelocity(
                Vector3.Direction(LastHit.AttackPosition, WorldPosition) * 350
            );
        }
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
    public Vector3 AttackPosition;

    public HitboxInfo Hitbox;
    public DamageInfo Damage;

    public HitLog(DamageInfo dmg, GameObject attacker) {
        Timestamp = Time.Now;
        Damage = dmg;

        AttackPosition = attacker.WorldPosition;
    }
}

