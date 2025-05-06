public partial class BrawlerComponent : Component {
    
    private void inputActions() {
        if (Input.Pressed("ResetCamera")) {
            BrawlerCamera.ResetPosition();
        }
    }

}