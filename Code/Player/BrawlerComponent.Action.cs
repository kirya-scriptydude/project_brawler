public partial class BrawlerComponent : Component {

    public ComboNode CurrentComboNode {get; private set;}
    [Property] public bool IsAction => CurrentComboNode.ClassName != "";
    private IBrawlerAction curAction;

    public IBrawlerAction[] ActionArray {get;} = [
        new Quickstep(),
        new Fist()
    ];

    public ComboNode ComboTreeRoot {get; set;} = ComboTree.Generate();
    

    public void ActionActivate(ComboNode node, bool activateIfBusy = false) {
        if (!activateIfBusy && IsAction) return;

        ActionStop(); 

        bool foundAction = false;
        foreach (IBrawlerAction action in ActionArray) {
            if (action.Name == node.ClassName) {
                CurrentComboNode = node;
                curAction = action;
                
                action.OnStart();

                foundAction = true;
                break;
            }
        }

        if (!foundAction) {
            Log.Warning($"action {node.ClassName} not found");
            return;
        }
    }

    public void ActionStop() {
        if (IsAction) {
            curAction.OnStop();
            CurrentComboNode = ComboTreeRoot;
        } 
    }

    /// <summary>
    /// set player for every action
    /// </summary>
    private void initializeActions() {
        foreach (IBrawlerAction action in ActionArray) {
            action.Player = this;
        }

        CurrentComboNode = ComboTreeRoot;
    }

    private void actionUpdate() {
        if (IsAction) curAction.OnUpdate();
    }
}