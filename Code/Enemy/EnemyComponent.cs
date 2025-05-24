using System;

/// <summary>
/// Base component for enemies
/// </summary>
[Group("Project Brawler")]
public partial class EnemyComponent : Component, IBrawler {

    public GameObject Object => GameObject;
    [Property] public SkinnedModelRenderer Model {get; set;}
    
    public bool MovementEnabled { get; set; } = true;
    public bool CanTraverseTree { get; set; } = true;

    [Property, RequireComponent] public HitboxHandler HitboxHandler { get; set; }
    [Property, RequireComponent] public HurtboxHandler HurtboxHandler { get; set; }
    [Property, RequireComponent] public ActionHandler ActionHandler { get; set; }

    [Property, RequireComponent] public NavMeshAgent Agent { get; set; }
    [Property, ReadOnly] public BrawlerComponent Player { get; set; }

    public bool StareAtPlayer { get; private set; } = true;
    public float DistanceToPlayer => Vector3.DistanceBetween(Player.WorldPosition, WorldPosition);
    

    [Property, ReadOnly, Group("Behaviour")] public EnemyState State { get; private set; } = EnemyState.Idle;
    

    /// <summary>
    /// This number rises up with time, clamping at a certain point. The higher the float, the more likely AI to act.
    /// Updates on EnemyState.Combat
    /// </summary>
    [Property, ReadOnly, Group("Behaviour")] public float WaitWeightFactor {get; private set;}

    public static readonly int PUSH_DISTANCE = 25;
    public static readonly int STOP_DISTANCE = 30;

    public static readonly float CHASE_DISTANCE = 250;

    private void stateIdle() {
        if (Player != null) {
            State = EnemyState.Chase;
        }
    }

    private void stateChase() {
        Agent.MoveTo(Player.WorldPosition);

        if (DistanceToPlayer < CHASE_DISTANCE) {
            State = EnemyState.Combat;
            Agent.Stop();
        }
    }

    protected override void OnStart() {
        //get all actions ready
        Player = Scene.GetComponentInChildren<BrawlerComponent>();

        Model.OnGenericEvent += delegate (SceneModel.GenericEvent e) {
            IBrawler.HookAnimgraphEvent(this, e);
        };
    }

    protected override void OnFixedUpdate() {
        
        if (DistanceToPlayer < PUSH_DISTANCE) {
            Agent.SetAgentPosition(LocalPosition + Vector3.Direction(Player.WorldPosition, WorldPosition) * 10);
        }

        if (HurtboxHandler.Hitstun || HurtboxHandler.Ragdolled) {
            SetVelocity(Vector3.Zero);
            Agent.Stop();
            return;
        }

        if (StareAtPlayer) {
            LocalRotation = Rotation.LookAt(Vector3.Direction(WorldPosition, Player.WorldPosition.WithZ(WorldPosition.z)));
        }
        
        switch (State) {
            case EnemyState.Idle:
                stateIdle();
                break;
            case EnemyState.Chase:
                stateChase();
                break;
            case EnemyState.Combat:
                stateCombat();
                break;
            default:
                break;
        }
    }

    public void SetVelocity(Vector3 velocity) {
        //dont go towards player if too close
        var dot = Vector3.Direction(WorldPosition, Player.WorldPosition).Dot(velocity);
        if (dot.AlmostEqual(COMBAT_VELOCITY_MOVESPEED, 1) && DistanceToPlayer < STOP_DISTANCE) {
            velocity = Vector3.Zero;
        }

        Agent.Velocity = velocity;
    }

    public Vector3 GetWishVelocity() {
        var Y = Random.Shared.Int(-1, 1);
        var X = Random.Shared.Int(-1, 1);
        return new Vector3(X, Y, 0) * LocalRotation;
    }
}

public enum EnemyState {
    Idle,
    Chase,
    Combat
}