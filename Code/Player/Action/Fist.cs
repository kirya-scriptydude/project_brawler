public class Fist : IBrawlerAction {
    public BrawlerComponent Player {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 1;
    public float LastTime {get; set;}

    public void OnStart() {}
    public void OnUpdate() {
        var time = Time.Now - LastTime;

        //combo period
        if (time > 0.65 && Player.CanTraverseTree == false) {
            Player.CanTraverseTree = true;
        }
    }

    public void OnStop() {}
}