/// <summary>
/// Evasion using quickstep, countering player's attack.
/// </summary>
public class Evade : IEnemyAction {
    public string Name {get;}
    public bool Condition() {
        return false;
    }

    public void OnStart() {

    }
}