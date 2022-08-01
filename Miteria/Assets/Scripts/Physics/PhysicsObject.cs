using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : ITickUpdate, ITickFixedUpdate
{

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1.0f;
    public Vector2 targetVelocity;
    public Vector2 velocity;
    public ContactFilter2D contactFilter;
    public bool noAirResistance;

    protected Transform playerPosition;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    private LayerMask mask;
    private int _distance = 1;
    private bool isContact;

    public void Init(Player player, GameObject _player, LayerMask _mask)
    {
        playerPosition = player.transform;
        rb2d = player.rb;
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(_player.layer));
        contactFilter.useLayerMask = true;
        mask = _mask;

        UpdateManager.ticks.Add(this);
        UpdateManager.fixedTicks.Add(this);
        Game.endGame += GameOver;
    }

    public void OnUpdate()
    {
        targetVelocity = Vector2.zero;
    }

    public void OnFixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = noAirResistance ? velocity.x : targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }



    private void Movement(Vector3 move, bool yMovement)
    {
        float distance = move.magnitude;


        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

            if (distance == -shellRadius)
            {
                OutBody();
            }

        }
        playerPosition.position += move.normalized * distance;
    }


    public void OutBody()
    {
        HitOutPlayer(new Ray2D(new Vector2(rb2d.position.x - _distance, rb2d.position.y), Vector2.right), Color.red);
        HitOutPlayer(new Ray2D(new Vector2(rb2d.position.x + _distance, rb2d.position.y), Vector2.left), Color.blue);
        HitOutPlayer(new Ray2D(new Vector2(rb2d.position.x, rb2d.position.y - _distance), Vector2.up), Color.gray);
        HitOutPlayer(new Ray2D(new Vector2(rb2d.position.x, rb2d.position.y + _distance), Vector2.down), Color.green);

        if (!isContact)
            _distance++;
    }


    private void HitOutPlayer(Ray2D ray, Color color)
    {
        var hit = Physics2D.Raycast(ray.origin, ray.direction, 1, mask);
        var stretchingHit = Physics2D.Raycast(ray.origin, ray.direction, _distance + 10, mask);


        if (stretchingHit)
        {
            if (hit)
                isContact = false;
            else
            {
                if (ray.origin.y != rb2d.position.y)
                {
                    //playerPosition.position = ray.origin.y > rb2d.position.y ? new Vector2(ray.origin.x, stretchingHit.collider.bounds.max.y + 1) :
                    //    new Vector2(ray.origin.x, stretchingHit.collider.bounds.min.y - 1);

                    playerPosition.position += (Vector3)(-ray.direction * _distance + (new Vector2(0, 0.5f) * ray.direction));
                }
                else
                {
                    playerPosition.position += (Vector3)(-ray.direction * _distance + (new Vector2(0.5f, 0) * ray.direction));
                }
                _distance = 1;
                isContact = true;
            }
        }

    }


    public void RemoveOverlap(Collision2D collision)
    {
        // If we're filtering out the collider we hit then ignore it.
        if (contactFilter.IsFilteringLayerMask(collision.collider.gameObject))
            return;

        // Calculate the collider distance.
        var colliderDistance = Physics2D.Distance(collision.otherCollider, collision.collider);

        // If we're overlapped then remove the overlap.
        // NOTE: We could also ensure we move out of overlap by the contact offset here.
        if (colliderDistance.isOverlapped)
            collision.otherRigidbody.position += colliderDistance.normal * colliderDistance.distance;
    }

    private void GameOver()
    {
        UpdateManager.ticks.Remove(this);
        UpdateManager.fixedTicks.Remove(this);
        Game.endGame -= GameOver;
    }
}
