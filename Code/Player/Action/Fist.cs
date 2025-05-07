public class Fist : IBrawlerAction {
    public BrawlerComponent Player {get; set;} 
    public string Name {get;} = "Fist";

    public float Duration {get; set;}
    public float LastTime {get; set;}

    public void OnStart() {}
    public void OnUpdate() {}
    public void OnStop() {}
}