using System;

public static class ReactionConditions {
    public static bool AlwaysTrue(EnemyComponent npc) {
        return true;
    }

    /// <summary>
    /// NPC will try to quickstep single stray hits, while being overwhelmed by combos.
    /// </summary>
    public static bool Quickstep(EnemyComponent npc) {
        float weight = Random.Shared.Float(0.1f);

        weight += npc.Evasion * 0.05f;
        weight += npc.WaitWeightFactor * 0.05f;

        if (npc.HurtboxHandler.ConsecutiveHits == 0) weight *= 2;

        Log.Info(weight);

        return weight >= 1;
    }
}