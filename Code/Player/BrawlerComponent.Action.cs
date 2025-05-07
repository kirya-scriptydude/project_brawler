public partial class BrawlerComponent : Component {
    /// <summary>
    /// Currently active action that character performs. "" == idle state.
    /// Do not change by itself - use ActionActivate() instead.
    /// </summary>
    [Property, ReadOnly] public string ActiveAction {get; private set;} = "";
    [Property] public bool IsAction => ActiveAction != "";
    private IBrawlerAction curAction;

    public IBrawlerAction[] ActionArray {get;} = [
        new Quickstep(),
        new FistAttack()
    ];

    public void ActionActivate(string actionName, bool activateIfBusy = false) {
        if (!activateIfBusy && IsAction) return;

        ActionStop(); 

        bool foundAction = false;
        foreach (IBrawlerAction action in ActionArray) {
            if (action.Name == actionName) {
                ActiveAction = action.Name;
                curAction = action;

                foundAction = true;
                break;
            }
        }

        if (!foundAction) {
            Log.Warning($"action {actionName} not found");
            return;
        }

        curAction.OnStart();
    }

    public void ActionStop() {
        if (IsAction) {
            curAction.OnStop();
            ActiveAction = "";
        } 
    }

    /// <summary>
    /// set player for every action
    /// </summary>
    private void initializeActions() {
        foreach (IBrawlerAction action in ActionArray) {
            action.Player = this;
        }
    }

    private void actionUpdate() {
        if (IsAction) curAction.OnUpdate();
    }
}