public partial class BrawlerComponent : Component {

    /// <summary>
    /// Combo node that is currently active. To change it please use ActionActivate.
    /// </summary>
    public ComboNode CurrentComboNode {get; private set;}
    public bool IsAction => CurrentComboNode.ClassName != "";
    private IBrawlerAction curAction;

    /// <summary>
    /// Current action can be moved into another one, thats next on the tree.
    /// </summary>
    public bool CanTraverseTree = true;

    public IBrawlerAction[] ActionArray {get;} = [
        new Quickstep(),
        new Fist(),
        new FistFinisher()
    ];

    /// <summary>
    /// Default root of the combo tree.
    /// </summary>
    public ComboNode ComboTreeRoot {get; set;} = ComboTree.Generate();
    
    /// <summary>
    /// Activate an action using ComboNode.
    /// </summary>
    /// <param name="node">Node that is a part of CurrentComboNode.Children</param>
    public void ActionActivate(ComboNode node) {
        if (!CanTraverseTree) return;

        if (!isNodeViable(node)) {
            Log.Warning($"tried using {node.Name} ({node.ClassName}) but it was not found.");
            return;
        }

        ActionStop(); 

        bool foundAction = false;
        foreach (IBrawlerAction action in ActionArray) {
            if (action.Name == node.ClassName) {
                CurrentComboNode = node;
                curAction = action;
                
                action.Enable();

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

    /// <summary>
    /// Stop currently selected action, execute .OnStop() and set CurrentComboNode to root.
    /// </summary>
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

    /// <summary>
    /// .OnUpdate() for IBrawlerAction
    /// </summary>
    private void actionUpdate() {
        if (IsAction) {
            curAction.OnUpdate();
            curAction.UpdateTimer();
        }
    }

    /// <summary>
    /// Checks if ComboNode can be done, according to currently selected node
    /// </summary>
    private bool isNodeViable(ComboNode node) {
        foreach (ComboNode child in CurrentComboNode.Children) {
            if (node == child) return true;   
        }

        return false;
    }
}