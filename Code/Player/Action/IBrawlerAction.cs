public interface IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;}

    /// <summary>
    /// Set to -1 to be infinite
    /// </summary>
    public float Duration {get; set;}
    public float LastTime {get; set;}

    public void Enable() {
        LastTime = Time.Now;
        OnStart();
    }

    public void UpdateTimer() {
        if (Duration < 0) return;
        
        var time = Time.Now - LastTime;

        if (time > Duration) Player.ActionStop();
    }

    public void OnStart();
    public void OnUpdate();
    public void OnStop();
}