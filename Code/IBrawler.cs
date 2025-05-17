/// <summary>
/// base interface for those who can use actions (IBrawlerAction) Players and NPC's alike.
/// </summary>
public interface IBrawler {
    public GameObject Object { get; }
    public SkinnedModelRenderer Model { get; set; }
    public HitboxHandler HitboxHandler { get; set; }

    public BrawlerHealth Health { get; set; }
    public InfoEntry MoveInfoEntry { get; set; }


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

    public static void HookAnimgraphEvent(IBrawler brawler, SceneModel.AnimTagEvent tagEvent) {
        if (brawler.HitboxHandler == null) {
            Log.Warning($"HitboxHandler on {brawler.Object} is not found");
            return;
        }

        if (tagEvent.Name == "DamageFrame") {
            var isEndFrame = tagEvent.Status == SceneModel.AnimTagStatus.End;
            brawler.HitboxHandler.Cast(
                ActionInfo.GetDamage(brawler.MoveInfoEntry),
                ActionInfo.GetHitbox(brawler.MoveInfoEntry),
                isEndFrame
            );
        }
    }

    public void Attack(AnimgraphAttackType attack) {
        Model.Parameters.Set("attackType", (int)attack);
        Model.Parameters.Set("b_isAttacking", true);
    }
}