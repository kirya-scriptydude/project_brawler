[Title( "Brawler Player" ), Group( "Project Brawler" ), Description( "Yakuza-like player controller" )]
public partial class BrawlerComponent : Component, IBrawler {
        public GameObject Object => GameObject;
        /// <summary>
        /// Active gameplay camera
        /// </summary>
        [Property] public GameObject Camera { get; set; }

        public BrawlerCamera BrawlerCamera { get; set; }


        [Property] public bool MovementEnabled { get; set; } = true;
        [Property] public GameObject Model {get; set;}

        protected override void OnStart() {
                initializeActions();
                BrawlerCamera = Camera.GetComponent<BrawlerCamera>();
        }

        protected override void OnUpdate() {
                if (Game.IsEditor) displayDebug();
                
                buildInput();
                actionControls();
                miscControls();
        }

        protected override void OnFixedUpdate() {
                if ( MovementEnabled ) move();
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