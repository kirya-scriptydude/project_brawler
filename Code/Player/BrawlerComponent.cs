[Title("Brawler Player"), Group("Project Brawler"), Description("Yakuza-like player controller")]
public partial class BrawlerComponent : Component {
    /// <summary>
    /// Active gameplay camera
    /// </summary>
    [Property] public GameObject Camera {get; set;}

	protected override void OnUpdate() {
        buildInput();
	}

    protected override void OnFixedUpdate() {
        move();
	}
}