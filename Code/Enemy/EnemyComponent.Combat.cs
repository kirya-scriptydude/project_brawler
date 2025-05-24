using System;

public partial class EnemyComponent : Component, IBrawler {

    static readonly float DIRECTION_DURATION = 4f;
    static readonly float COMBAT_VELOCITY_MOVESPEED = 50;

    private float lasTimeChangedVelocity = Time.Now;
    private int dirIndex = 0;
    private Vector3[] availableDirs = [
        Vector3.Forward,
        Vector3.Left,
        Vector3.Right,
        Vector3.Zero
    ];

    private void stateCombat() {
        WaitWeightFactor = Math.Clamp(WaitWeightFactor + 0.005f, 0.5f, 5);

        if (DistanceToPlayer > CHASE_DISTANCE) {
            State = EnemyState.Chase;
        }

        //change walk velocity after an amount of time
        if (Time.Now - lasTimeChangedVelocity > DIRECTION_DURATION && Random.Shared.Int(3) == 0) {
            lasTimeChangedVelocity = Time.Now;
            dirIndex = Random.Shared.Int(availableDirs.Length - 1);
        }

        SetVelocity(availableDirs[dirIndex] * COMBAT_VELOCITY_MOVESPEED * LocalRotation);

        foreach (var node in ActionHandler.CurrentNode.Children) {
            if (node.NonPlayableCondition(this)) {
                ActionHandler.Use(node);
                WaitWeightFactor = 0;

                StareAtPlayer = false;
            }
        }

        if (!ActionHandler.IsAction && StareAtPlayer == false) StareAtPlayer = true;
    }

}