using UnityEngine;

public class QSlerp : MonoBehaviour
{
    public Transform from;
    public Transform to;

    private float timeCount =  0.0f;

    void Update()
    {
        Vector3 center = (from.position + to.position) * 0.5F;
        center -= new Vector3(0, 1, 0);
        Vector3 riseRelCenter = from.position - center;
        Vector3 setRelCenter = to.position - center;

        transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, timeCount);
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, timeCount);
        timeCount = timeCount + Time.deltaTime;
    }
}
