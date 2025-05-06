[Title("Brawler Player"), Group("Project Brawler"), Description("Yakuza-like player controller")]
public partial class BrawlerComponent : Component {
        /// <summary>
        /// Active gameplay camera
        /// </summary>
        [Property] public GameObject Camera {get; set;}

        public BrawlerCamera BrawlerCamera {get; set;}


        [Property] public bool MovementEnabled {get; set;} = true;

	protected override void OnStart() {
                initializeActions();
                BrawlerCamera = Camera.GetComponent<BrawlerCamera>();
	}

	protected override void OnUpdate() {
                buildInput();
                actionControls();
                miscControls();
	}

        protected override void OnFixedUpdate() {
                if (MovementEnabled) move();
                actionUpdate();
        }
}