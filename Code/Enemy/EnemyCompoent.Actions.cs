public partial class EnemyComponent : Component {

    /// <summary>
    /// big list of all available actions.
    /// </summary>
    public static List<IEnemyAction> AllActions = new List<IEnemyAction>() {
        new Evade()
    };

}