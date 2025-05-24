using System;

public static class NonPlayableConditions {

    public static bool Attack(EnemyComponent npc) {
        float weight = Random.Shared.Float(0, 0.1f);

        weight += npc.Aggression * 0.05f;
        weight *= npc.WaitWeightFactor;

        if (npc.DistanceToPlayer > 75) weight = 0;

        //Log.Info(weight);

        return weight >= 1;
    }

}