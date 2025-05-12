/// <summary>
/// Base component for enemies
/// </summary>
[Group("Project Brawler")]
public partial class EnemyComponent : Component, IBrawler {

    public GameObject Object => GameObject;
    public bool MovementEnabled { get; set; } = true;
    public bool CanTraverseTree { get; set; } = true;

    [Property, RequireComponent] public NavMeshAgent Agent { get; set; }
    [Property, ReadOnly] public BrawlerComponent Player { get; set; }

    public bool StareAtPlayer { get; private set; } = true;
    public float DistanceToPlayer => Vector3.DistanceBetween(Player.WorldPosition, WorldPosition);

    public string CurrentAction { get; private set; } = "";
    public bool IsAction => CurrentAction != "";
    private IBrawlerAction curAction;

    public IReadOnlyList<IBrawlerAction> AllAvailableActions { get; } = new List<IBrawlerAction>() {
        new Quickstep()
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
        }
    }

    private void stateCombat() {
        if (DistanceToPlayer > CHASE_DISTANCE) {
            State = EnemyState.Chase;
        }
        
        if (IsAction) {
            curAction.UpdateTimer();
            curAction.OnUpdate();
        } else {
            //idle
            foreach (var action in actionClassArray) {
                if (action.NonPlayableCondition()) {
                    CurrentAction = action.Name;
                    curAction = action;
                    action.OnStart();
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
    }

    protected override void OnFixedUpdate() {
        if (StareAtPlayer) {
            LocalRotation = Rotation.LookAt(Vector3.Direction(WorldPosition, Player.WorldPosition));
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
    }

    public Vector3 GetWishVelocity() {
        return new Vector3();
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