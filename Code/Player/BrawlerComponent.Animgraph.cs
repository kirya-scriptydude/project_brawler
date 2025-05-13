public partial class BrawlerComponent : Component, IBrawler {
    private void updateAnimgraph() {
        Model.Parameters.Set("b_walking", Controller.Velocity.Length > 1);
    }

    private void handleAnimationEvents(SceneModel.AnimTagEvent tagEvent) {
        if (tagEvent.Name == "DamageFrame") {
            Log.Info("DAMAGE FRAME");
        }
    }
}