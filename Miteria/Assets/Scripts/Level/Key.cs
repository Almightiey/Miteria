using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _) && !Game.mainObjects.keyManager.Keys.ContainsKey(id))
        {
            Game.mainObjects.keyManager.Keys.Add(id, this);
            Destroy(gameObject);
        }
    }
}
