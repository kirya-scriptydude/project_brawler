using System;

public class Quickstep : IBrawlerAction {
    public IBrawler Brawler {get; set;}
    public string Name {get;} = "Quickstep";

    public float Duration {get; set;} = 0.8f;
    public float CancelDuration {get; set;} = 0.2f;
    public float LastTime {get; set;}

    private Vector3 velocity = new();
    private BrawlerComponent Player;

    public void OnStart() {
        Player = Brawler.Object.GetComponent<BrawlerComponent>();
        velocity = Brawler.GetWishVelocity() * BrawlerComponent.MOVESPEED * 1.5f;

        if (Player != null) {
            Player.ModelAnimScale = new Vector3(1, 1, 0.1f);
            Player.MovementEnabled = false;
        }

    }

    public void OnUpdate() {
        Brawler.SetVelocity(velocity);
        velocity *= 0.93f;

        //Player.Controller.Move();
        if (Player != null) {
            Player.ModelAnimScale = new Vector3(1, 1, 0.9f);
        }
        
    }

    public void OnStop() {
        if (Player != null) {
            Player.MovementEnabled = true;
        }
    }

    public bool NonPlayableCondition(EnemyComponent npc) {
        float weight = Random.Shared.Float(1, 20);

        if (npc.Player.IsPerfomingAttack() || npc.QuickstepInNeutral) weight += 100;
        if (npc.DistanceToPlayer < 100) weight += 100;
        
        weight *= npc.WaitWeightFactor;
        weight += npc.Evasion * 50;

        if (weight > 1000) {
            Log.Info($"*** DASHED ({weight}) ***");
        }
        return weight >= 1000;
    }
}