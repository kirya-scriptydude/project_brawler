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

    public BrawlerHealth Health {get; set;} = new();
    public AttackType MoveAttackType {get; set;}
    [Property] public HitboxHandler HitboxHandler { get; set; }

    [Property, RequireComponent] public NavMeshAgent Agent { get; set; }
    [Property, ReadOnly] public BrawlerComponent Player { get; set; }

    public bool StareAtPlayer { get; private set; } = true;
    public float DistanceToPlayer => Vector3.DistanceBetween(Player.WorldPosition, WorldPosition);
    

    [Property, ReadOnly, Group("Behaviour")] public EnemyState State { get; private set; } = EnemyState.Idle;
    

    /// <summary>
    /// This number rises up with time, clamping at a certain point. The higher the float, the more likely AI to attack.
    /// Updates on EnemyState.Combat
    /// </summary>
    [Property, ReadOnly, Group("Behaviour")] public float WaitWeightFactor {get; private set;}

    public static readonly int PUSH_DISTANCE = 40;

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

    private void stateCombat() {
        WaitWeightFactor = Math.Clamp(WaitWeightFactor + 0.005f, 0.5f, 5);

        if (DistanceToPlayer > CHASE_DISTANCE) {
            State = EnemyState.Chase;
        }

        updateActions();
    }

    protected override void OnStart() {
        //get all actions ready
        initActions();
        Player = Scene.GetComponentInChildren<BrawlerComponent>();

        Model.OnGenericEvent += delegate (SceneModel.GenericEvent e) {
            IBrawler.HookAnimgraphEvent(this, e);
        };
    }

    protected override void OnFixedUpdate() {
        if (Health.Dead) {
            GameObject.Destroy();
            return;
        }

        if (StareAtPlayer) {
            LocalRotation = Rotation.LookAt(Vector3.Direction(WorldPosition, Player.WorldPosition.WithZ(WorldPosition.z)));
        }

        //player too close, push npc
        if (DistanceToPlayer < PUSH_DISTANCE) {
            Agent.SetAgentPosition(LocalPosition + Vector3.Direction(Player.WorldPosition, WorldPosition)* 10);
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
        Agent.Velocity = velocity;
    }

    public Vector3 GetWishVelocity() {
        var Y = Random.Shared.Int(-1, 1);
        var X = Random.Shared.Int(-1, 1);
        return new Vector3(X, Y, 0) * LocalRotation;
    }

    public void StopAction() {
        if (IsAction) {
            curAction.OnStop();
            CurrentAction = "";
            CanTraverseTree = true;
        } 
    }
}

public enum EnemyState {
    Idle,
    Chase,
    Combat
}