public static class ComboTree {
    /// <summary>
    /// Turn an array of nodes into a chain (linked list), where it's chaining from start to end of array.
    /// </summary>
    /// <param name="nodes">nodes to chain</param>
    /// <returns>First node of the chained list</returns>
    public static ComboNode Chain(ComboNode[] nodes) {
        for (int i = nodes.Length - 1; i > -1; i--) {
            int parent = i - 1;
            if (parent < 0) break;

            nodes[parent].Children.Add(nodes[i]);   
        }

        return nodes[0];
    }

    /// <summary>
    /// Generates default tree. TODO: Create options to customize it.
    /// </summary>
    /// <returns>Root node</returns>
    public static ComboNode Generate() {
        var startList = new List<ComboNode>();
        var root = new ComboNode("root", ActionInputButton.None, "");

        {
            var fist01 = new ComboNode("fist01", ActionInputButton.Fist, "Fist");
            var fist02 = new ComboNode("fist02", ActionInputButton.Fist, "Fist");
            var fist03 = new ComboNode("fist03", ActionInputButton.Fist, "Fist");
            var fist04 = new ComboNode("fist04", ActionInputButton.Fist, "Fist");
            startList.Add(Chain([fist01, fist02, fist03, fist04]));

            //im sorry
            fist01.Children.Add(
                new ComboNode("fist01_finisher", ActionInputButton.Kick, "FistFinisher")
            );

            fist02.Children.Add(
                new ComboNode("fist02_finisher", ActionInputButton.Kick, "FistFinisher")
            );

            fist03.Children.Add(
                new ComboNode("fist03_finisher", ActionInputButton.Kick, "FistFinisher")
            );
            fist04.Children.Add(
                new ComboNode("fist04_finisher", ActionInputButton.Kick, "FistFinisher")
            );
        }

        {
            var quickstep = new ComboNode("quickstep", ActionInputButton.Quickstep, "Quickstep");
            startList.Add(quickstep);
        }

        root.Children = startList;
        return root;
    }
}