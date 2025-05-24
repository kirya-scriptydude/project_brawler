using System;

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

    /// <summary>
    /// Function that is used by NPC's to determine if they should use this node.
    /// </summary>
    public Func<EnemyComponent, bool> NonPlayableCondition = delegate (EnemyComponent npc) {
        return false;
    };

    public HitboxInfo HitboxInfo { get; set; } = new(32);
    public DamageInfo DamageInfo { get; set; } = new(50);

    public ComboNode(string name, ActionInputButton input, string className) {
        Name = name;
        Button = input;
        ClassName = className;
    }
}