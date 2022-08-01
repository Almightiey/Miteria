using UnityEngine;

public class WaterSpawn : MonoBehaviour
{
    public int count;
    public GameObject dropObject;


    private void Start()
    {
    }

    [ContextMenu("Spawn Water")]
    public void SpawnWater()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(dropObject, transform.position + (Vector3)Random.insideUnitCircle * 1.5f, Quaternion.identity, transform.parent);
        }
    }
}
