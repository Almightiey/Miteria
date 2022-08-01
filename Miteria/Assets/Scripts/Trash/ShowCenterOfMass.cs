using UnityEngine;

public class ShowCenterOfMass : MonoBehaviour
{
    public Vector2 centerOfMass;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(1, 0) * 40, ForceMode2D.Force);
    }

    void Update()
    {
        rb.centerOfMass = centerOfMass;
    }
}
