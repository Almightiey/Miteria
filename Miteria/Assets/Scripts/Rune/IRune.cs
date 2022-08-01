public interface IRune
{
    public float timeLifeEffect { get; set; }
    public delegate void RuneActive();
    public event RuneActive runeActive;
    public void RuneEffect();
    public void Init();
}
