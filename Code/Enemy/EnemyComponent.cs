/// <summary>
/// Base component for enemies
/// </summary>
[Group("Project Brawler")]
public partial class EnemyComponent : Component {

    [Property, RequireComponent] public NavMeshAgent Agent {get; set;}
    [Property, ReadOnly] public BrawlerComponent Player {get; set;}

    [Property,ReadOnly] public EnemyState State {get; private set;} = EnemyState.Idle;
    public bool StareAtPlayer {get; private set;} = true;

    public float DistanceToPlayer => Vector3.DistanceBetween(Player.WorldPosition, WorldPosition);

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

	protected override void OnStart() {
        Player = Scene.GetComponentInChildren<BrawlerComponent>();
	}

	protected override void OnFixedUpdate() {
        if (StareAtPlayer) {
            LocalRotation = Rotation.LookAt(Vector3.Direction(WorldPosition, Player.WorldPosition));
        }

        switch(State) {
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

}

public enum EnemyState {
    Idle,
    Chase,
    Combat
}