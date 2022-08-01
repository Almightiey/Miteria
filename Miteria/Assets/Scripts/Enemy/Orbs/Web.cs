using UnityEngine;

public class Web : DistantionAtack, ITickUpdate
{
    private bool isRetains;
    public float time;
    public float next;
    public float distantion;
    public LayerMask layerMask;
    private bool isCorrect;


    private void Start()
    {
        animator = GetComponent<Animator>();
        to = new Vector3(Game.player.transform.position.x, Game.player.transform.position.y, Game.player.transform.position.z);
        UpdateManager.ticks.Add(this);
        Game.endGame += EndGame;
    }

    public void OnUpdate()
    {
        if (isShoot)
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.up, distantion, layerMask);

            if (hit)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 180);
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                if (hit.collider.TryGetComponent(out HitPlayer _))
                {
                    HitEffect();
                    if (next > Time.time)
                        return;
                }
                isRetains = true;
                Game.isPlayerStop = false;
                animator.SetBool(nameof(isRetains), isRetains);
                transform.rotation = Quaternion.identity;
                Destroy(gameObject, time / 1000);
                UpdateManager.ticks.Remove(this);
                Game.endGame -= EndGame;
                return;
            }
            if (!isCorrect)
            {
                animator.SetBool(nameof(isShoot), isShoot);
                transform.parent = null;
                Vector3 center = (from.position + to) * 0.5F;
                center -= Vector3.up;
                Vector3 riseRelCenter = from.position - center;
                Vector3 setRelCenter = to - center;
                transform.rotation = Quaternion.Slerp(from.rotation, Quaternion.Euler(0, 0, -90 * transform.localScale.x), speed);
                transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, speed);
                transform.position += center;
            }
            if (transform.position.y <= to.y)
            {
                isCorrect = true;
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1, 0), speed);
            }
            speed += Time.deltaTime;
        }
    }


    public void HitEffect()
    {
        var player = Game.player;
        if (!isRetains)
        {
            next = Time.time + time;
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y - transform.localScale.y);
        }
        transform.rotation = Quaternion.identity;
        Game.isPlayerStop = true;
        isRetains = true;
        animator.SetBool(nameof(isRetains), isRetains);
    }


    private void EndGame()
    {
        Game.isPlayerStop = false;
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }
}
