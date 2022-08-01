using UnityEngine;

public class TrajectoryRender : MonoBehaviour
{
    private LineRenderer LineRenderer;
    public int count;
    public bool isHit;
    public LayerMask mask;
    public LayerMask enemyMask;
    public Vector3 endPoint;

    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();    
    }

    public void ShowLineTrajectory(Vector3 origin, Vector3 finalPoint)
    {
        Vector3[] points = { origin, finalPoint };
        LineRenderer.positionCount = points.Length;
        LineRenderer.SetPositions(points);
    }


    public void ShowCurveTrajectoty(Vector2 origin, Vector2 speed)
    {
        Vector3[] points = new Vector3[count];
        LineRenderer.positionCount = points.Length;
        isHit = false;
        for (int i = 0; i < points.Length; i++)
        {
            if (!isHit)
            {
                float time = i * 0.1f;

                points[i] = origin + speed * time + Physics2D.gravity * time * time / 2;
                points[i] = new Vector3(points[i].x, points[i].y, 0);
            }
            else
            {
                points[i] = new Vector3(points[i - 1].x, points[i - 1].y, 0);
                endPoint = points[i];
            }
            Ray2D ray = new Ray2D(points[i], Vector2.down);
            isHit = Physics2D.Raycast(ray.origin, ray.direction, 0.1f, mask);
            if (Physics2D.Raycast(ray.origin, ray.direction, 0.1f, enemyMask))
            {
                LineRenderer.endColor = Color.red;
            }
            else
            {
                LineRenderer.endColor = new Vector4(0.3f, 1.0f, 0.0f, 1.0f);
            }

        }

        LineRenderer.SetPositions(points);

    }
}
