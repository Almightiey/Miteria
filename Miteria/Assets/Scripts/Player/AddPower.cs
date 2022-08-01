using System.Collections.Generic;
using UnityEngine;

public class AddPower : MonoBehaviour
{
    public PlayerState newUpdateState;
    public PlayerState newFixedState;
    public PlayerState newLateState;



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent(out HitPlayer hitPlayer))
        {
            GetState(newUpdateState, hitPlayer.playerActor.playerUpdateMethod, hitPlayer.playerActor);
            GetState(newFixedState, hitPlayer.playerActor.playerFixedUpdateMethod, hitPlayer.playerActor);
            GetState(newLateState, hitPlayer.playerActor.playerLateUpdateMethod, hitPlayer.playerActor);
        }
    }


    private void GetState(PlayerState state, List<PlayerState> playerStates, PlayerActor player)
    {
        if (state != null)
        {
            playerStates.Add(state);
            state.player = Game.player;
            state.Init();
            Destroy(gameObject);
        }
    }

}
