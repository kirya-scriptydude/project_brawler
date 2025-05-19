using System;

public class Quickstep : IBrawlerAction {
    public IBrawler Brawler {get; set;}
    public string Name {get;} = "Quickstep";

    public float Duration {get; set;} = 0.7f;
    public float CancelDuration {get; set;} = 0.2f;
    public float LastTime {get; set;}

    private float velocity = new();
    private Vector3 wishVelocity = new();

    public void OnStart() {
        Brawler.MovementEnabled = false;

        velocity = BrawlerComponent.MOVESPEED * 2.5f;
        wishVelocity = Brawler.GetWishVelocity().Normal;
    }

    public void OnUpdate() {
        var vel = wishVelocity * velocity;

        Brawler.SetVelocity(vel);
        velocity *= 0.80f;
    }

    public void OnStop() {
        Brawler.MovementEnabled = true;
    }

    public bool NonPlayableCondition(EnemyComponent npc) {
        float weight = Random.Shared.Float(1, 20);

        if (npc.Player.IsPerfomingAttack() || npc.QuickstepInNeutral) weight += 100;
        if (npc.DistanceToPlayer < 100) weight += 100;
        
        weight *= npc.WaitWeightFactor;
        weight += npc.Evasion * 50;

        return weight >= 1000;
    }
}