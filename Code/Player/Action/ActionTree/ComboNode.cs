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

    public ComboNode[] Children = {};
    public ActionInputButton Button;

    public ComboNode(string name, ActionInputButton input, string className) {
        Name = name;
        Button = input;
        ClassName = className;
    }
}