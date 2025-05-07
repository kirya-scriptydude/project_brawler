//code for displaying useful debug info on the screen.
public partial class BrawlerComponent : Component {

    private void displayDebug() {
        DebugOverlay.ScreenText(new Vector2(25, 25), 
        $"""
        MoveDirection - {MoveDirection};  MoveDirectionAngled - {MoveDirectionAngled};
        AnalogLook - {AnalogLook};
        LockOn - {Input.Down("LockOn")};

        Action - {IsAction};
        CanTraverseTree - {CanTraverseTree}
        Name - {CurrentComboNode.ClassName}
        {CurrentComboNode.Name}
        """, 
        10, TextFlag.LeftTop
        );
    }
}