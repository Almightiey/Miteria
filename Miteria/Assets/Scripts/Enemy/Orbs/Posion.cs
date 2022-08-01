using System.Threading.Tasks;
using UnityEngine;

public class Posion : DistantionAtack, ITickUpdate
{
    public float time;
    public int delay;
    public float next;
    public float damage;
    public bool isActive;
    public float distantion;
    public LayerMask layerMask;
    public bool isCorrect;
    private bool isEndGame;
    private float direction;

    private void Start()
    {
        animator = GetComponent<Animator>();
        to = !isToRandom ? new Vector3(Game.player.transform.position.x, Game.player.transform.position.y, Game.player.transform.position.z) :
            new Vector3(transform.position.x + Random.Range(-10, 10), Game.player.transform.position.y, Game.player.transform.position.z);
        UpdateManager.ticks.Add(this);
        isActive = false;
        Game.endGame += EndGame;
        direction = to.x - transform.position.x;
    }

    public void OnUpdate()
    {
        if (isShoot && !isActive)
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.up, distantion, layerMask);

            if (hit)
            {
                transform.localRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 180);
                transform.localScale = new Vector3(0.5f * transform.localScale.x, 0.5f, 0.5f);
                if (hit.collider.TryGetComponent(out HitPlayer _) && !isActive)
                {
                    isActive = true;
                    animator.SetBool(nameof(isActive), isActive);
                    HitEffect();
                    return;
                }
                if (hit.collider.TryGetComponent(out Posion posion))
                    posion.next = Time.time + time;
                isActive = true;
                animator.SetBool(nameof(isActive), isActive);
                Destroy(gameObject, 2.0f);
                Delete();
                return;
            }
            ActTravel();
        }
    }


    private void ActTravel()
    {
        transform.parent = null;
        if (from.localRotation != Quaternion.identity && !isCorrect)
        {
            Vector3 center = (from.position + to) * 0.5F;
            center -= Vector3.up;
            Vector3 riseRelCenter = from.position - center;
            Vector3 setRelCenter = to - center;
            transform.rotation = Quaternion.Slerp(from.rotation, Quaternion.Euler(0, 0, -90 * transform.localScale.x), speed);
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, speed);
            transform.position += center;
        }
        else
        {
            transform.localScale = Vector3.one;
            Vector3 difference = from.position - to;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);
            to = Decriment(to);
            transform.position = Vector3.Lerp(from.position, to, speed);
        }
        if (transform.position.y <= to.y)
        {
            isCorrect = true;
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 1, 0), speed);
        }
        speed += Time.deltaTime;
    }

    private Vector3 Decriment(Vector3 vector)
    {
        return new Vector3(vector.x + direction / 10, --vector.y, vector.z);
    }

    private void Delete()
    {
        isEndGame = true;
        UpdateManager.ticks.Remove(this);
        isActive = false;
        Game.endGame -= EndGame;
    }

    public async void HitEffect()
    {
        UpdateManager.ticks.Remove(this);
        next = Time.time + time;
        var player = Game.player;
        while (next > Time.time && player.health > 0 && isActive)
        {
            player.health -= damage - (damage / 100 * player.hitDamage.armor);
            player.hitDamage.ChangeHealth();
            await Task.Delay(delay);
        }
        if (!isEndGame)
        {
            Game.endGame -= EndGame;
            Destroy(gameObject);
        }
    }


    private void EndGame()
    {
        Delete();
    }
}
