/// <summary>
/// Handles object's hurtbox and response to hits. Controls hitstun using animgraph provided by model.
/// </summary>
[Group("Project Brawler")]
public class HurtboxHandler : Component {
    public SkinnedModelRenderer Model { get; set; }

    public HitstunType LastType { get; set; } = HitstunType.Generic;

    public bool Hitstun { get; private set; } = false;
    public bool Ragdolled { get; private set; } = false;


    protected override void OnStart() {
        Model = GameObject.GetComponent<IBrawler>().Model;

        Model.OnAnimTagEvent += tagEvents;
        Model.OnGenericEvent += genericEvents;
    }

    public void Hurt(DamageInfo dmg) {
        //todo actually deal damage
        if (dmg.DoHitstun) {
            Model.Parameters.Set("hitstunType", (int)dmg.Hitstun);
            Model.Parameters.Set("b_hit", true);
        }
    }

    private void tagEvents(SceneModel.AnimTagEvent e) {
        if (e.Status == SceneModel.AnimTagStatus.Fired) return;

        switch (e.Name) {
            case "Hitstun":
                Hitstun = e.Status == SceneModel.AnimTagStatus.Start;
                break;
            case "Ragdolled":
                Ragdolled = e.Status == SceneModel.AnimTagStatus.Start;
                break;
        }

    }

    private void genericEvents(SceneModel.GenericEvent e) { }
}

