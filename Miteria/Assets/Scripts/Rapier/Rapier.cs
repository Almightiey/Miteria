using UnityEngine;

public class Rapier : MonoBehaviour
{
    public TrailRenderer trail;
    public float Force;
    public float distance;
    public LayerMask wtIsSolid;


    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    internal bool isCollider;
    private bool isShot;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        trail.gameObject.SetActive(false);
        isCollider = false;
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, wtIsSolid);
        if (hitInfo.collider != null)
        {
            isCollider = true;
            rb.simulated = false;
            boxCollider.enabled = false;
            if (hitInfo.collider.GetComponent<EnemyEasy>() != null)
            {
                transform.parent = hitInfo.collider.transform;
            }
            return;
        }
        Rotate();
    }

    public void Shoot(Vector3 direction)
    {
        trail.gameObject.SetActive(true);
        rb.simulated = true;
        rb.AddForce(direction * Force, ForceMode2D.Impulse);
        isShot = true;
    }


    private void Rotate()
    {
        if (isShot)
        {
            var direction = rb.velocity;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
