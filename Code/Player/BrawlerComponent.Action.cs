public partial class BrawlerComponent : Component {

    public ComboNode CurrentComboNode {get; private set;}
    [Property] public bool IsAction => CurrentComboNode.ClassName != "";
    private IBrawlerAction curAction;

    /// <summary>
    /// Current action can be moved into another one, thats next on the tree.
    /// </summary>
    public bool CanTraverseTree = true;

    public IBrawlerAction[] ActionArray {get;} = [
        new Quickstep(),
        new Fist()
    ];

    public ComboNode ComboTreeRoot {get; set;} = ComboTree.Generate();
    

    public void ActionActivate(ComboNode node) {
        if (!CanTraverseTree) return;

        ActionStop(); 

        bool foundAction = false;
        foreach (IBrawlerAction action in ActionArray) {
            if (action.Name == node.ClassName) {
                CurrentComboNode = node;
                curAction = action;
                
                action.OnStart();

                foundAction = true;
                CanTraverseTree = false;
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
            CanTraverseTree = true;
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