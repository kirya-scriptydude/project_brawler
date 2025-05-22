using System;

/// <summary>
/// base interface for those who can use actions (IBrawlerAction) Players and NPC's alike.
/// </summary>
public interface IBrawler {
    public GameObject Object { get; }
    public SkinnedModelRenderer Model { get; set; }
    public HitboxHandler HitboxHandler { get; set; }

    public BrawlerHealth Health { get; set; }
    public AttackType MoveAttackType { get; set; }


    public bool MovementEnabled { get; set; }
    /// <summary>
    /// Current action can be moved into another one, thats next on the tree. If it's npc, just means if any move can be used
    /// </summary>
    public bool CanTraverseTree { get; set; }
    public void SetVelocity(Vector3 velocity);
    /// <summary>
    /// Get velocity that controller wishes to move in.
    /// </summary>
    /// <returns>Vector3 movement direction.</returns>
    public Vector3 GetWishVelocity();
    public void StopAction();

    public static void HookAnimgraphEvent(IBrawler brawler, SceneModel.GenericEvent tagEvent) {
        if (brawler.HitboxHandler == null) {
            Log.Warning($"HitboxHandler on {brawler.Object} is not found");
            return;
        }
        
        if (tagEvent.Type == "DamageFrames") {
            brawler.HitboxHandler.Cast(
                ActionInfo.GetDamage(brawler.MoveAttackType),
                ActionInfo.GetHitbox(brawler.MoveAttackType),
                Math.Clamp(tagEvent.Int, 1, 256)
            );
        }
    }

    /// <summary>
    /// set animgraph parameters and MoveAttackType according to AttackType provided
    /// </summary>
    /// <param name="attack">desired attack</param>
    public void Attack(AttackType attack) {
        Model.Parameters.Set("action", (int)attack);
        Model.Parameters.Set("b_action", true);
        MoveAttackType = attack;
    }
}