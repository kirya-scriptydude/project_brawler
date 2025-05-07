using System.Runtime.Serialization.Formatters;

public interface IBrawlerAction {
    public BrawlerComponent Player {get; set;}
    public string Name {get;}

    /// <summary>
    /// Set to -1 to be infinite
    /// </summary>
    public float Duration {get; set;}
    /// <summary>
    /// When this amount of time passes, enable cancelling moves and moving thru combo tree. -1 to disable.
    /// </summary>
    public float CancelDuration {get; set;}
    public float LastTime {get; set;}

    public void Enable() {
        LastTime = Time.Now;
        OnStart();
    }

    public void UpdateTimer() {
        if (Duration < 0) return;
        
        var time = Time.Now - LastTime;

        if (CancelDuration >= 0 && Player.CanTraverseTree == false) {
            if (time > CancelDuration) Player.CanTraverseTree = true;
        }

        if (time > Duration) Player.ActionStop();
    }

    public void OnStart();
    public void OnUpdate();
    public void OnStop();
}