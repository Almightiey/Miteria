using UnityEngine;

public abstract class State : ScriptableObject
{
    public Enemy Person;
    public EnemyMiddle MiddlePerson;


    public bool isFinished { get; set; }
    public virtual void Init(Enemy person) { Person = person; }
    public abstract void Run();
}
