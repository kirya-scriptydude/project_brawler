public class BrawlerHealth {
    public int MaxHP {get; set;} = 1000;
    public int HP {get; set;} = 1000;
    public bool Dead => HP <= 0;

    /// <summary>
    /// Takes this amount of damage. If value is negative then heal instead.
    /// </summary>
    /// <param name="num"></param>
    public void Inflict(int num) {
        HP -= num;
        if (HP > MaxHP) HP = MaxHP;
    }
}