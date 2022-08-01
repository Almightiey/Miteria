using UnityEngine;

public class Parallax : MonoBehaviour, ITickUpdate
{
    public Transform followingTarget;
    public float parallaxSpeed;
    public bool enableVecrticalParallax;
    private Vector3 targetPreviousPosition;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (!followingTarget)
            followingTarget = mainCamera.transform;

        targetPreviousPosition = followingTarget.position;
    }

    private void OnEnable()
    {
        Game.endGame += EndGame;
        UpdateManager.ticks.Add(this);
    }


    private void OnDisable()
    {
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }

    private void EndGame()
    {
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }

    public void OnUpdate()
    {
        var delta = followingTarget.position - targetPreviousPosition;
        if (!enableVecrticalParallax)
            delta.y = 0;
        
        delta.z = 0;

        targetPreviousPosition = followingTarget.position;
        transform.position += delta * parallaxSpeed;
    }
}
