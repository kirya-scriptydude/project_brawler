public partial class BrawlerComponent : Component, IBrawler {
    private void updateAnimgraph() {
        Model.Parameters.Set("b_walking", Controller.Velocity.Length > 1);
    }

    private void handleAnimationEvents(SceneModel.AnimTagEvent tagEvent) {
        if (HitboxHandler == null) {
            Log.Warning($"HitboxHandler on {GameObject} is not found");
            return;
        }

        if (tagEvent.Name == "DamageFrame") {
            var isEndFrame = tagEvent.Status == SceneModel.AnimTagStatus.End;
            HitboxHandler.Cast(
                ActionInfo.GetDamage(MoveInfoEntry),
                ActionInfo.GetHitbox(MoveInfoEntry),
                isEndFrame
            );
        }
    }
}