using System.Threading.Tasks;
using UnityEngine;

public class AirRapController
{
    public float distance;
    public float speed;
    public float distancePlayerOfRapier;
    public Vector2 point;
    private Vector2 startPosition;
    private Animator animator { get; set; }
    internal bool isPlayerTooFar;
    internal bool isCameBack;
    internal bool isStop;
    internal RaycastHit2D hitInfo;
    internal AttackPlayer rapierAtackPlayer;
    private Player player;

    public void Init()
    {
        speed = 50.0f;
        distance = 0.1f;
        distancePlayerOfRapier = 10.0f;
        player = Game.player;
        animator = player.rapierController.rapier.GetComponent<Animator>();
        rapierAtackPlayer = player.rapierController.rapier.GetComponentInChildren<AttackPlayer>();
    }

    public void Restart()
    {
        startPosition = player.rapierController.rapier.transform.position;
        isPlayerTooFar = false;
        isStop = false;
        isCameBack = false;
    }

    public void OnUpdate(GameObject rapier, LayerMask wtIsSolid)
    {
        hitInfo = Physics2D.Raycast(rapier.transform.position, rapier.transform.up, distance, wtIsSolid);
        if (hitInfo.collider != null)
        {
            rapierAtackPlayer.enabled = false;
            var hit = Physics2D.Raycast(new Vector2(rapier.transform.position.x, rapier.transform.position.y + 1.5f), rapier.transform.up, distance, wtIsSolid);
            if (!hit)
            {
                //var direction = (rapier.transform.position.x - player.transform.position.x) / Mathf.Abs(rapier.transform.position.x - player.transform.position.x);
                var x = rapier.transform.position.x - hitInfo.collider.bounds.min.x < 0.7f ? hitInfo.collider.bounds.min.x + 1.0f :
                    hitInfo.collider.bounds.max.x - rapier.transform.position.x < 0.7f ? hitInfo.collider.bounds.max.x - 1.0f :
                    rapier.transform.position.x;
                point = new Vector2(x, hitInfo.collider.bounds.max.y + 1);
                //point = new Vector2(rapier.transform.position.x, hitInfo.collider.bounds.max.y);
            }
            return;
        }
        point = rapier.transform.position;

        if (speed > 0)
        {
            if (Vector2.Distance(startPosition, rapier.transform.position) > distancePlayerOfRapier && !isStop)
            {
                animator.SetBool("isPlayerTooFar", true);
                BackRapier(rapier);
            }
            else if (!isPlayerTooFar && !isStop)
            {
                rapier.transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
        }
    }

    public async void BackRapier(GameObject rapier)
    {
        await Task.Delay(500);
        CameBack(rapier);
    }

    public void CameBack(GameObject rapier)
    {
        if (rapier != null && !isStop)
        {
            rapier.transform.position = Vector2.Lerp(rapier.transform.position, player.transform.position, speed * 2 * Time.deltaTime);
            isPlayerTooFar = true;
            if (Vector2.Distance(player.transform.position, rapier.transform.position) < 0.5f)
            {
                isCameBack = true;
            }
        }
    }
}
