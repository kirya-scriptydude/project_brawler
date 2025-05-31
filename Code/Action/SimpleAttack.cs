using System;

/// <summary>
/// generic attack class for most of the needed strikes.
/// </summary>
public class SimpleAttack : IBrawlerAction {
    public IBrawler Brawler { get; set; }

    public string Name => "SimpleAttack";

    public float Duration { get; set; } = 1.5f;
    public float CancelDuration { get; set; } = 1.5f;
    public float LastTime { get; set; }

    Vector3 velocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;

        var anim = (AnimationType)Enum.Parse(typeof(AnimationType), Brawler.ActionHandler.CurrentNode.Name);
        Brawler.Model.Parameters.Set("genericAttackType", (int)anim);

        Brawler.PerformActionAnimation(AttackType.GenericAttack);

        velocity = Brawler.ActionHandler.CurrentNode.HitboxInfo.DashVelocity * Brawler.Object.LocalRotation;
    }

    public void OnUpdate() {
        if (Brawler.HitboxHandler.HitboxActive) {
            Brawler.SetVelocity(velocity);
            velocity = velocity.LerpTo(Vector3.Zero, 0.15f);
        } else Brawler.SetVelocity(Vector3.Zero);
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }

    public enum AnimationType {
        unrefined_attack,
        unrefined_combo
    }
    
}