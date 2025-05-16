[Title("Brawler Player"), Group("Project Brawler"), Description("Yakuza-like player controller")]
public partial class BrawlerComponent : Component, IBrawler {
        public GameObject Object => GameObject;
        /// <summary>
        /// Active gameplay camera
        /// </summary>
        [Property] public GameObject Camera { get; set; }
        [Property] public HitboxHandler HitboxHandler { get; set; }

        public BrawlerHealth Health {get; set;} = new();
        public InfoEntry MoveInfoEntry {get; set;} = InfoEntry.Default;

        public BrawlerCamera BrawlerCamera { get; set; }


        [Property] public bool MovementEnabled { get; set; } = true;
        [Property] public SkinnedModelRenderer Model { get; set; }

        protected override void OnStart() {
                initializeActions();
                Model.OnAnimTagEvent += delegate (SceneModel.AnimTagEvent e) {
                        IBrawler.HookAnimgraphEvent(this, e);
                };

                BrawlerCamera = Camera.GetComponent<BrawlerCamera>();
        }

        protected override void OnUpdate() {
                if (Game.IsEditor) displayDebug();

                buildInput();
                actionControls();
                updateAnimgraph();
                miscControls();
        }

        protected override void OnFixedUpdate() {
                if (MovementEnabled) move();
                actionUpdate();
        }

        public void SetVelocity(Vector3 velocity) {
                Controller.Velocity = velocity;
                Controller.Move();
        }

        public Vector3 GetWishVelocity() {
                return MoveDirectionAngled;
        }
}