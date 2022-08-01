using UnityEngine;

public class ParabolaMove : MonoBehaviour
{
    protected float Animation;
    private Vector3 start;
    private Vector3 end;
    private Vector3 center;
    private float height;

    private GameObject root;
    private GameObject A;
    private GameObject B;
    private GameObject C;

    private ParabolaController parabolaController;

    private void Start()
    {
        start = transform.position;
        center = transform.position;
        end = transform.position;
        height = 0;

        root = new GameObject();
        A = InitPoint();
        B = InitPoint();
        C = InitPoint();
        parabolaController = GetComponent<ParabolaController>();
        parabolaController.ParabolaRoot = root;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    start = transform.position;
        //    var to = Game.player.transform.position;
        //    end = new Vector3(to.x + (to.x - start.x), start.y, 0);
        //    height = to.y;
        //}

        //Animation += Time.deltaTime;

        //Animation = Animation % 5.0f;

        //transform.position = MathParabola.Parabola(start, end, height, Animation / 5);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            A.transform.position = transform.position;
            B.transform.position = Game.player.transform.position;
            C.transform.position = new Vector3(B.transform.position.x + (B.transform.position.x - A.transform.position.x), A.transform.position.y, 0);
            parabolaController.FollowParabola();
        }

    }


    private GameObject InitPoint()
    {
        var point = new GameObject();
        point.transform.parent = root.transform;
        point.transform.position = transform.position;
        return point;
    }
}
