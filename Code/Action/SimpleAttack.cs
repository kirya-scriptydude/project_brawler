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

    public void OnStart() {
        Brawler.MovementEnabled = false;

        var anim = (AnimationType)Enum.Parse(typeof(AnimationType), Brawler.ActionHandler.CurrentNode.Name);
        Brawler.Model.Parameters.Set("genericAttackType", (int)anim);
        
        Brawler.PerformActionAnimation(AttackType.GenericAttack);
    }

    public void OnUpdate() {
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }

    public enum AnimationType {
        unrefined_attack,
        unrefined_combo
    }
    
}