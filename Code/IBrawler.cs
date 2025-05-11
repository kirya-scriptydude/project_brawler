/// <summary>
/// base interface for those who can use actions (IBrawlerAction) Players and NPC's alike.
/// </summary>
public interface IBrawler {
    public GameObject Object {get;}


    public bool MovementEnabled {get; set;}
    /// <summary>
    /// Current action can be moved into another one, thats next on the tree. If it's npc, just means if any move can be used
    /// </summary>
    public bool CanTraverseTree {get; set;}
    public void SetVelocity(Vector3 velocity);
    /// <summary>
    /// Get velocity that controller wishes to move in.
    /// </summary>
    /// <returns>Vector3 movement direction.</returns>
    public Vector3 GetWishVelocity();
    public void StopAction();
}