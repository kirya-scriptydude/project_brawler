public partial class EnemyComponent : Component, IBrawler {

    public IReadOnlyList<IBrawlerAction> AllAvailableActions { get; } = new List<IBrawlerAction>() {
        new Quickstep(),
        new FistFinisher()
    };

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

    public string CurrentAction { get; private set; } = "";
    public bool IsAction => CurrentAction != "";
    private IBrawlerAction curAction;


    private void initActions() {
        var actionArr = new List<IBrawlerAction>();
        
         foreach (var str in ActionList) {
            foreach (var inst in AllAvailableActions) {
                inst.Brawler = this;
                if (inst.Name == str) actionArr.Add(inst);
            }
        }

        actionClassArray = actionArr.ToArray();
    }

    private void updateActions() {
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
    

}