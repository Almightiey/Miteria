using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/MoveFly")]
public class MoveFly : State
{
    private GameObject root;
    public GameObject A;
    private GameObject B;
    private GameObject C;
    private bool isAttackingStart;

    private ParabolaController parabolaController;

    public override void Init(Enemy person)
    {
        base.Init(person);

        root = root == null ? new GameObject() : root;
        A = A == null ? InitPoint() : A;
        B = B == null ? InitPoint() : B;
        C = C == null ? InitPoint() : C;
        if (parabolaController == null)
        {
            parabolaController = new ParabolaController();
        }
        parabolaController.transform = person.transform;
        parabolaController.Autostart = false;
        parabolaController.Animation = false;
        parabolaController.Speed = Person.speedEnemy;
        parabolaController.ParabolaRoot = root;
        parabolaController.Init();
        isAttackingStart = false;
    }

    public override void Run()
    {
        if (isFinished)
        {
            Person.isNoFlip = false;
            return;
        }
        parabolaController.OnUpdate();

        if (Person.nextDelayMove > Time.time)
        {
            Person.animation.SetInteger("State", 0);
            isAttackingStart = false;
            Person.isNoFlip = false;
        }
        else if (!isAttackingStart)
        {
            Person.animation.SetInteger("State", 2);
            A.transform.position = Person.transform.position;
            B.transform.position = Game.player.transform.position;
            C.transform.position = new Vector3(B.transform.position.x + (B.transform.position.x - A.transform.position.x), A.transform.position.y, 0);
            parabolaController.FollowParabola();
            isAttackingStart = true;
            Person.isNoFlip = true;
        }
        else if (isAttackingStart && Mathf.Abs(Vector3.Distance(Person.transform.position, C.transform.position)) < 0.5f)
        {
            Person.nextDelayMove = Time.time + Random.Range(Person.minDelayMove, Person.maxDelayMove);
        }
        //else if (to == Vector3.zero && !isTargetComplete)
        //{
        //    from = Person.transform;
        //    to = Game.player.transform.position;
        //    secondTo = new Vector3(to.x + Mathf.Abs(to.x - from.position.x) * Person.transform.localScale.x, from.position.y, 0);
        //}
        //else
        //{
        //    Person.isNoFlip = true;
        //    Vector3 center = (from.position - to) * 0.5F;
        //    center -= Vector3.up;
        //    Vector3 riseRelCenter = from.position - center;
        //    Vector3 setRelCenter = to - center;
        //    //Person.transform.rotation = Quaternion.Slerp(from.rotation, Quaternion.Euler(0, 0, 90 * Person.transform.localScale.x), Person.speedEnemy);
        //    Person.transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, Person.speedEnemy);
        //    Person.transform.position += center;
        //    Person.animation.SetInteger("State", 2);

        //    if (Mathf.Abs(Vector3.Distance(from.position, to)) < 0.5f && !isTargetComplete)
        //    {
        //        isTargetComplete = true;
        //        to = secondTo;
        //    }
        //    else if (Mathf.Abs(Vector3.Distance(from.position, to)) < 0.5f)
        //    {
        //        isTargetComplete = false;
        //        Person.nextDelayMove = Time.time + Random.Range(Person.minDelayMove, Person.maxDelayMove);
        //        to = Vector3.zero;
        //    }
        //}
        isFinished = !Person.isMoving || Person.isDialog || Person.isDeath || Game.player.isDeath;
    }



    private GameObject InitPoint()
    {
        var point = new GameObject();
        point.transform.parent = root.transform;
        point.transform.position = Person.transform.position;
        return point;
    }
}
