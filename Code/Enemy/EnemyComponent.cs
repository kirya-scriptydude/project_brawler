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

    public string CurrentAction { get; private set; } = "";
    public bool IsAction => CurrentAction != "";
    private IBrawlerAction curAction;

    public IReadOnlyList<IBrawlerAction> AllAvailableActions { get; } = new List<IBrawlerAction>() {
        new Quickstep(),
        new FistFinisher()
    };

    [Property, ReadOnly, Group("Behaviour")] public EnemyState State { get; private set; } = EnemyState.Idle;
    /// <summary>
    /// List of actions that enemy is capable of doing.
    /// ActionList has a priority of a list's order (higher up - first to evaluate)
    /// </summary>
    [Property, Group("Behaviour")]
    public List<string> ActionList { get; set; } = new() {
        "Quickstep"
    };
    /// <summary>
    /// Premade array with all the actions mentioned in ActionList.
    /// </summary>
    private IBrawlerAction[] actionClassArray;

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

        if (IsAction) {
            curAction.UpdateTimer();
            curAction.OnUpdate();
        } else {
            //idle
            foreach (var action in actionClassArray) {
                if (action.NonPlayableCondition(this)) {
                    CurrentAction = action.Name;
                    curAction = action;
                    action.OnStart();
                    
                    //reset wait factor
                    WaitWeightFactor = 0.7f;
                    break;
                }
            }
        }
    }

    protected override void OnStart() {
        var actionArr = new List<IBrawlerAction>();

        //get all actions ready
        foreach (var str in ActionList) {
            foreach (var inst in AllAvailableActions) {
                inst.Brawler = this;
                if (inst.Name == str) actionArr.Add(inst);
            }
        }

        actionClassArray = actionArr.ToArray();
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