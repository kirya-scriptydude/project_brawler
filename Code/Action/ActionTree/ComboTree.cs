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
            nodes[i].TreeLevel = i;
        }

        return nodes[0];
    }

    /// <summary>
    /// Generates default tree. TODO: Create options to customize it.
    /// </summary>
    /// <returns>Root node</returns>
    public static ComboNode Generate() {
        var root = new ComboNode("root", ActionInputButton.None, "");

        {
            var fist01 = new ComboNode("fist01", ActionInputButton.Fist, "Fist");
            var fist02 = new ComboNode("fist02", ActionInputButton.Fist, "Fist");
            var fist03 = new ComboNode("fist03", ActionInputButton.Fist, "Fist");
            var fist04 = new ComboNode("fist04", ActionInputButton.Fist, "Fist");
            Chain([root, fist01, fist02, fist03, fist04]);

            var fist_Finisher = new ComboNode("fist_finisher", ActionInputButton.Kick, "FistFinisher");
            var finisherDmg = new DamageInfo(250, DamageType.Heavy, DamageSource.Kick);
            finisherDmg.Hitstun = HitstunType.Knockdown;
            fist_Finisher.DamageInfo = finisherDmg;

            //im sorry
            fist01.Children.Add(fist_Finisher);

            fist02.Children.Add(fist_Finisher);

            fist03.Children.Add(fist_Finisher);

            fist04.Children.Add(fist_Finisher);
        }

        {
            var quickstep = new ComboNode("quickstep", ActionInputButton.Quickstep, "Quickstep");
            root.Children.Add(quickstep);
        }
        return root;
    }

    public static ComboNode GenerateNPC() {
        var root = new ComboNode("root", ActionInputButton.None, "");

        var attack = new ComboNode("unrefined_attack", ActionInputButton.Quickstep, "SimpleAttack");
        attack.NonPlayableCondition = NonPlayableConditions.Attack;

        var dmginfo = new DamageInfo(100, DamageType.Heavy, DamageSource.Fist);
        dmginfo.Hitstun = HitstunType.Ground;
        attack.DamageInfo = dmginfo;

        root.Children.Add(attack);


        var combo = new ComboNode("unrefined_combo", ActionInputButton.Quickstep, "SimpleAttack");
        combo.NonPlayableCondition = NonPlayableConditions.Attack;
        root.Children.Add(combo);

        return root;
    }
}