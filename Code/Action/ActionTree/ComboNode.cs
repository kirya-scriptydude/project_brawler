public enum ActionInputButton {
    None,
    Quickstep,
    Fist,
    Kick,
    Grab
}

public class ComboNode {
    public string Name {get;}
    /// <summary>
    /// C# Class that's responsible for this node's functionality (ex. Quickstep)
    /// </summary>
    public string ClassName {get;}

    public List<ComboNode> Children = new();
    public ActionInputButton Button;

    /// <summary>
    /// Indicates what level this node is on. It's 0 if root.
    /// </summary>
    public int TreeLevel { get; set; } = 0;

    public ComboNode(string name, ActionInputButton input, string className) {
        Name = name;
        Button = input;
        ClassName = className;
    }
}