[Title("Brawler Player"), Group("Project Brawler"), Description("Yakuza-like player controller")]
public partial class BrawlerComponent : Component {
    /// <summary>
    /// Active gameplay camera
    /// </summary>
    [Property] public GameObject Camera {get; set;}
    public BrawlerCamera BrawlerCamera {get; set;}
	protected override void OnStart() {
        BrawlerCamera = Camera.GetComponent<BrawlerCamera>();
	}
	protected override void OnUpdate() {
        buildInput();
        inputActions();
	}

    protected override void OnFixedUpdate() {
        move();
	}
}