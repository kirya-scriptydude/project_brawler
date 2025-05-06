public interface IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;}

    public void OnStart();
    public void OnUpdate();
    public void OnStop();
}