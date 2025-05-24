//code for displaying useful debug info on the screen.
public partial class BrawlerComponent : Component {

    private void displayDebug() {
        DebugOverlay.ScreenText(new Vector2(25, 25),
        $"""
        MoveDirection - {MoveDirection};  MoveDirectionAngled - {MoveDirectionAngled};
        AnalogLook - {AnalogLook};
        LockOn - {Input.Down("LockOn")};

        Action - {ActionHandler.IsAction};
        CanTraverseTree - {ActionHandler.CanTraverseTree}
        Name - {ActionHandler.CurrentNode.ClassName}
        {ActionHandler.CurrentNode.Name}
        """,
        10, TextFlag.LeftTop
        );

        if (LockOnTarget != null && Input.Down("LockOn")) {
            var npc = LockOnTarget.GetComponent<IBrawler>();
            DebugOverlay.ScreenText(new Vector2(-20, 20),
                $"""
                Hitstun - {npc.HurtboxHandler.Hitstun}
                Ragdoll - {npc.HurtboxHandler.Ragdolled}
                """,
                10, TextFlag.RightTop
            );
        }
    }
}