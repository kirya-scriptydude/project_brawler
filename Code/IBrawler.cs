using System;

/// <summary>
/// base interface for those who can use actions (IBrawlerAction) Players and NPC's alike.
/// </summary>
public interface IBrawler {
    public GameObject Object { get; }
    public SkinnedModelRenderer Model { get; set; }
    
    public ActionHandler ActionHandler { get; set; }
    public HitboxHandler HitboxHandler { get; set; }
    public HurtboxHandler HurtboxHandler { get; set; }
    public StatsHandler Stats { get; set; }


    public bool MovementEnabled { get; set; }
    public void SetVelocity(Vector3 velocity);
    public Vector3 GetVelocity();
    /// <summary>
    /// Get velocity that controller wishes to move in.
    /// </summary>
    /// <returns>Vector3 movement direction.</returns>
    public Vector3 GetWishVelocity();

    public static void HookAnimgraphEvent(IBrawler brawler, SceneModel.GenericEvent tagEvent) {
        if (brawler.HitboxHandler == null) {
            Log.Warning($"HitboxHandler on {brawler.Object} is not found");
            return;
        }
        
        if (tagEvent.Type == "DamageFrames") {
            brawler.HitboxHandler.Cast(
                brawler.ActionHandler.CurrentNode.DamageInfo,
                brawler.ActionHandler.CurrentNode.HitboxInfo,
                Math.Clamp(tagEvent.Int, 1, 256)
            );
        }
    }

    /// <summary>
    /// set animgraph parameters and MoveAttackType according to AttackType provided
    /// </summary>
    /// <param name="attack">desired attack</param>
    public void PerformActionAnimation(AttackType attack) {
        Model.Parameters.Set("action", (int)attack);
        Model.Parameters.Set("b_action", true);
    }
}