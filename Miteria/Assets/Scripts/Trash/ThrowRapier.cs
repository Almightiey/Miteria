using UnityEngine;

public class ThrowRapier : MonoBehaviour
{
    public GameObject rapierPrefub;
    public float power = 100;

    public TrajectoryRender Trajectory;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void Update()
    {
        float enter;


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
        Vector3 mouseInWorld = ray.GetPoint(enter);

        Vector3 speed = (mouseInWorld - transform.position) * power;
        transform.rotation = Quaternion.LookRotation(speed);
        
        Trajectory.ShowCurveTrajectoty(transform.position, speed);

        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody2D rapier = Instantiate(rapierPrefub, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rapier.AddForce(speed, ForceMode2D.Impulse);
            float rotZ = Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg;
            rapier.transform.rotation = Quaternion.Lerp(rapier.transform.rotation,
                Quaternion.Euler(0f, 0f, rotZ + 90), Time.deltaTime);
        }
    }

}
