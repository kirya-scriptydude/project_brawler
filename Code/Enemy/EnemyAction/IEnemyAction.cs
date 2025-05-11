/// <summary>
/// Base interface for all enemy actions (quicksteps, attacking, defending, etc.)
/// </summary>
public interface IEnemyAction {
    public string Name {get;}
    public bool Condition();
    public void OnStart();
}