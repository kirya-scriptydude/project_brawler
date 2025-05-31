using System;

public partial class EnemyComponent : Component, IBrawler {

    static readonly float COMBAT_MOVESPEED = 50;
    /// <summary>
    /// range that's npc will try to keep in combat.
    /// </summary>
    static readonly float COMBAT_FOOTSIES_RANGE = 150;
    /// <summary>
    /// How much time should pass on average before npc would pick new walk direction again
    /// </summary>
    static readonly float COMBAT_WALK_DECISION_TIME = 1f;

    /// <summary>
    /// Mood that influences enemy actions and decisions. Make them play defensively or go offense 
    /// </summary>
    public MoodType Mood { get; private set; } = MoodType.Aggressive;

    private Vector3 wishVelocity = new();

    float curDecisionTime = COMBAT_WALK_DECISION_TIME;

    private void stateCombat() {
        if (DistanceToPlayer > CHASE_DISTANCE) {
            State = EnemyState.Chase;
            return;
        }

        if (ActionHandler.IsAction) {
            WaitWeightFactor = 0;
            return;
        }
        
        // todo choose another moods
        Mood = MoodType.Aggressive;

        var footsies = COMBAT_FOOTSIES_RANGE;
        if (Mood == MoodType.Aggressive) footsies /= 2;

        curDecisionTime -= Time.Delta;
        if (curDecisionTime <= 0) {
            wishVelocityDecision(footsies);
            curDecisionTime = COMBAT_WALK_DECISION_TIME + Random.Shared.Float(-(COMBAT_WALK_DECISION_TIME / 2), COMBAT_WALK_DECISION_TIME / 2);
        }

        SetVelocity(wishVelocity * COMBAT_MOVESPEED * LocalRotation);

        // choose new action yipeee
        foreach (var node in ActionHandler.CurrentNode.Children) {
            if (node.NonPlayableCondition(this)) {
                ActionHandler.Use(node);
            }
        }

        WaitWeightFactor = Math.Clamp(WaitWeightFactor + Time.Delta, 0.5f, 5);
    }

    private void wishVelocityDecision(float footsieRange) {
        var strafe = Random.Shared.Int(-1, 1);
        var x = DistanceToPlayer > footsieRange ? 1 : -1;

        var multiplier = Math.Clamp(DistanceToPlayer / footsieRange, 0, 1);

        wishVelocity = new(x, strafe, 0);
        wishVelocity *= multiplier;
    }


    public enum MoodType {
        Aggressive,
        Defensive
    }
}