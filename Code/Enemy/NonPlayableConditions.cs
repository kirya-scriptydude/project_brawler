using System;

public static class NonPlayableConditions {

    public static bool Attack(EnemyComponent npc) {
        float weight = Random.Shared.Float(0, 0.1f);

        weight += npc.Aggression * 0.05f;
        weight *= npc.WaitWeightFactor;

        if (npc.DistanceToPlayer > 90) weight /= 2;
        if (npc.Player.HurtboxHandler.IFrame) weight /= 2;

        return weight >= 1;
    }

}