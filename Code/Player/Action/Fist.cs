public class Fist : IBrawlerAction {
    public BrawlerComponent Player {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;} = 1;
    public float CancelDuration {get; set;} = 0.7f;
    public float LastTime {get; set;}

    public void OnStart() {}
    public void OnUpdate() {}

    public void OnStop() {}
}