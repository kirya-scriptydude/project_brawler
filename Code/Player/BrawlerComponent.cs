[Title("Brawler Player"), Group("Project Brawler"), Description("Yakuza-like player controller")]
public partial class BrawlerComponent : Component, IBrawler {
        public GameObject Object => GameObject;
        /// <summary>
        /// Active gameplay camera
        /// </summary>
        [Property] public GameObject Camera { get; set; }
        [Property, RequireComponent] public HitboxHandler HitboxHandler { get; set; }
        [Property, RequireComponent] public HurtboxHandler HurtboxHandler { get; set; }
        [Property, RequireComponent] public ActionHandler ActionHandler { get; set; }

        public BrawlerCamera BrawlerCamera { get; set; }


        [Property] public bool MovementEnabled { get; set; } = true;
        [Property] public SkinnedModelRenderer Model { get; set; }

        protected override void OnStart() {
                Model.OnGenericEvent += delegate (SceneModel.GenericEvent e) {
                        IBrawler.HookAnimgraphEvent(this, e);
                };

                BrawlerCamera = Camera.GetComponent<BrawlerCamera>();
        }

        protected override void OnUpdate() {
                if (Game.IsEditor) displayDebug();

                buildInput();
                actionControls();
                miscControls();

                moveAnimate();
        }

        protected override void OnFixedUpdate() {
                if (!HurtboxHandler.NotStunned) return;
                if (MovementEnabled) move();
                moveRotation();
        }

        public void SetVelocity(Vector3 velocity) {
                Controller.Velocity = velocity;
                Controller.Move();
        }
        public Vector3 GetVelocity() {
                return Controller.Velocity;
        }

        public Vector3 GetWishVelocity() {
                return MoveDirectionAngled;
        }
}