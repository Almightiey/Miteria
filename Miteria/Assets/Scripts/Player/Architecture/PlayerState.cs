using UnityEngine;

public abstract class PlayerState : ScriptableObject
{
    public Player player;
    public abstract void Run();

    public virtual void Init() { }
}
