using UnityEngine;

public class DistantionAtack : MonoBehaviour
{
    public bool isShoot;
    public float speed;
    protected Animator animator;
    public bool isToRandom;
    internal Transform from;
    protected Vector3 to { get; set; }
}
