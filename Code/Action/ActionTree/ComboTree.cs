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
    /// Generates default tree. TODO: Create options to customize it. Oh god what have i done
    /// </summary>
    /// <returns>Root node</returns>
    public static ComboNode Generate() {
        var root = new ComboNode("root", ActionInputButton.None, "");

        var fistDmg = new DamageInfo(50, DamageType.Light, DamageSource.Fist);
        fistDmg.KnockbackMultiplier = 4f;
        fistDmg.PlayHitSound = true;
        var fistHitbox = new HitboxInfo(40);
        fistHitbox.DashVelocity = Vector3.Forward * 300;

        var fist01 = new ComboNode("fist01", ActionInputButton.Fist, "Fist");
        fist01.DamageInfo = fistDmg;
        fist01.HitboxInfo = fistHitbox;

        var fist02 = new ComboNode("fist02", ActionInputButton.Fist, "Fist");
        fist02.DamageInfo = fistDmg;
        fist02.HitboxInfo = fistHitbox;

        var fist03 = new ComboNode("fist03", ActionInputButton.Fist, "Fist");
        fist03.DamageInfo = fistDmg;
        fist03.HitboxInfo = fistHitbox;

        var fist04 = new ComboNode("fist04", ActionInputButton.Fist, "Fist");
        fist04.DamageInfo = fistDmg;
        fist04.HitboxInfo = fistHitbox;
        Chain([root, fist01, fist02, fist03, fist04]);

        var fist_Finisher = new ComboNode("fist_finisher", ActionInputButton.Kick, "FistFinisher");

        var finisherDmg = new DamageInfo(250, DamageType.Heavy, DamageSource.Kick);
        finisherDmg.Hitstun = HitstunType.Knockdown;
        finisherDmg.PlayHitSound = true;
        finisherDmg.HitSound = "sounds/heavy-impact.sound";

        var finisherHitbox = new HitboxInfo(40);
        finisherHitbox.Length = 25;
        finisherHitbox.DashVelocity = Vector3.Forward * 600;
        


        fist_Finisher.DamageInfo = finisherDmg;
        fist_Finisher.HitboxInfo = finisherHitbox;

        //im sorry
        fist01.Children.Add(fist_Finisher);
        fist02.Children.Add(fist_Finisher);
        fist03.Children.Add(fist_Finisher);
        fist04.Children.Add(fist_Finisher);

        var quickstep = new ComboNode("quickstep", ActionInputButton.Quickstep, "Quickstep");
        root.Children.Add(quickstep);

        fist01.Children.Add(quickstep);
        fist02.Children.Add(quickstep);
        fist03.Children.Add(quickstep);
        fist04.Children.Add(quickstep);

        var block = new ComboNode("block", ActionInputButton.Block, "BlockPlayer");
        root.Children.Add(block);

        return root;
    }

    public static ComboNode GenerateNPC() {
        var root = new ComboNode("root", ActionInputButton.None, "");

        var finisherHitbox = new HitboxInfo(32);
        finisherHitbox.Length = 16;
        finisherHitbox.DashVelocity = Vector3.Forward * 400;

        var attack = new ComboNode("unrefined_attack", ActionInputButton.Quickstep, "SimpleAttack");
        attack.HitboxInfo = finisherHitbox;
        attack.NonPlayableCondition = NonPlayableConditions.Attack;

        var lightDmginfo = new DamageInfo(50, DamageType.Light, DamageSource.Fist);
        lightDmginfo.HitSound = "sounds/light-impact-alt.sound";
        lightDmginfo.PlayHitSound = true;

        var dmginfo = new DamageInfo(100, DamageType.Heavy, DamageSource.Fist);
        dmginfo.Hitstun = HitstunType.Juggle;
        dmginfo.HitSound = "sounds/light-impact-alt.sound";
        dmginfo.PlayHitSound = true;
        attack.DamageInfo = dmginfo;

        //root.Children.Add(attack);


        var combo = new ComboNode("unrefined_combo", ActionInputButton.Quickstep, "SimpleAttack");
        combo.NonPlayableCondition = NonPlayableConditions.Attack;
        combo.DamageInfo = lightDmginfo;
        combo.HitboxInfo = finisherHitbox;
        //root.Children.Add(combo);

        // reaction nodes
        {
            var quickstepReact = new ComboNode("quickstep", ActionInputButton.Quickstep, "Quickstep");
            quickstepReact.ReactionCondition = ReactionConditions.Quickstep;
            root.Children.Add(quickstepReact);
        }

        return root;
    }
}