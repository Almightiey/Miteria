using UnityEngine;

public class SecondsTargetCamera : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _))
        {
            var player = Game.player;
            if (!player.rapierController.dropRapier)
            {
                player.rapierController.rapierTransform.position = transform.position;
                player.rapierController.inTheAdditionalTargetAreaOfTheCamera = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _))
        {
            var player = Game.player;
            if (!player.rapierController.dropRapier)
            {
                player.rapierController.inTheAdditionalTargetAreaOfTheCamera = false;
            }
        }
    }

}
