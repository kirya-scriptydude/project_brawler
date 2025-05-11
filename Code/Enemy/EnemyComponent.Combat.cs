using System;

public partial class EnemyComponent : Component {

    public CombatState StateCombat {get; private set;} = CombatState.Neutral;

    private float sidestep = 0f;
    private float lastChangedSidestep = Time.Now;

    public static readonly float CLOSE_QUARTERS_DISTANCE = 150f;

    private void stateCombat_Neutral() {
        if (DistanceToPlayer > CLOSE_QUARTERS_DISTANCE) {
            Agent.MoveTo(Player.WorldPosition);
        } 

        if (Time.Now - lastChangedSidestep > 5) {
            sidestep = Random.Shared.Float(-1, 1);
            lastChangedSidestep = Time.Now;
        }
        

        Agent.Velocity = LocalRotation.Left * (sidestep * 60);
    }

    private void stateCombat_Attack() {

    }

    private void stateCombat_Defend() {

    }

    private void stateCombat() {
        if (DistanceToPlayer > CHASE_DISTANCE) {
            State = EnemyState.Chase;
        }

        switch (StateCombat) {
            case CombatState.Neutral:
                stateCombat_Neutral();
                break;
            case CombatState.Attack:
                stateCombat_Attack();
                break;
            case CombatState.Defend:
                stateCombat_Defend();
                break;
            default:
                StateCombat = CombatState.Neutral;
                break;
        }
    }

    public void StopAction() {}
    public void SetVelocity(Vector3 velocity) => Agent.Velocity = velocity;
    public Vector3 GetWishVelocity() => Agent.WishVelocity;

}

public enum CombatState {
    Neutral,
    Attack,
    Defend

}