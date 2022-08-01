using UnityEngine;

public class Door : MonoBehaviour
{
    public int id;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out HitPlayer _) && Game.mainObjects.keyManager.Keys.ContainsKey(id))
        {
            Game.mainObjects.keyManager.Keys.Remove(id);
            Destroy(gameObject);
        }
    }
}
