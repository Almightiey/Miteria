using UnityEngine;

public interface IQuest
{
    public int Id { get; set; }
    public GameObject gift { get; set; }
    public int Count { get; set; }
    public int MaxCount { get; set; }
    public bool complete { get; set; }
}
