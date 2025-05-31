/// <summary>
/// Component that handles special actions and inputs (attacks and stuff) for bots and players alike.
/// </summary>
[Group("Project Brawler")]
public class ActionHandler : Component {

    /// <summary>
    /// use npc default tree
    /// </summary>
    [Property] public bool UseNPCtree { get; set; } = false;

    public IBrawler Brawler { get; set; }

    /// <summary>
    /// If true, you can use moves thats next on tree.
    /// </summary>
    public bool CanTraverseTree { get; set; } = true;
    public bool IsAction => CurrentNode.ClassName != "";

    public ComboNode Root { get; private set; }
    public ComboNode CurrentNode { get; private set; }
    private IBrawlerAction curAction;

    /// <summary>
    /// hardcoded list of all available actions (theres really should be a better way)
    /// </summary>
    private IBrawlerAction[] allActions = [
        new Quickstep(),
        new Fist(),
        new FistFinisher(),
        new BlockPlayer(),
        new BlockNPC(),
        
        new SimpleAttack()
    ];


    protected override void OnStart() {
        Brawler = GameObject.GetComponent<IBrawler>();
        
        Root = UseNPCtree ? ComboTree.GenerateNPC() : ComboTree.Generate();
        CurrentNode = Root;
    }

    protected override void OnFixedUpdate() {
        if (IsAction) {
            curAction.OnUpdate();
            curAction.UpdateTimer();
        }
	}


    public void Use(ComboNode node) {
        if (!CanTraverseTree) return;

        if (!isNodeViable(node)) {
            Log.Warning($"tried using {node.Name} ({node.ClassName}) but it was not found.");
            return;
        }

        //making sure we dont have other actions running
        Stop();

        bool foundAction = false;
        foreach (IBrawlerAction action in allActions) {
            if (action.Name == node.ClassName) {
                CurrentNode = node;
                curAction = action;

                action.Brawler = Brawler;
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
    public void Stop() {
        if (IsAction) {
            curAction.OnStop();
            CurrentNode = Root;
            CanTraverseTree = true;
        } 
    }


    /// <summary>
    /// Checks if ComboNode can be done, according to currently selected node
    /// </summary>
    private bool isNodeViable(ComboNode node) {
        foreach (ComboNode child in CurrentNode.Children) {
            if (node == child) return true;
        }

        return false;
    }
}