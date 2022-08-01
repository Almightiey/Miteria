using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static List<ITickUpdate> ticks = new List<ITickUpdate>();
    public static List<ITickFixedUpdate> fixedTicks = new List<ITickFixedUpdate>();
    public static List<ITickLateUpdate> lateTicks = new List<ITickLateUpdate>();


    void Update()
    {
        for (int i = 0; i < ticks.Count; i++)
        {
            ticks[i].OnUpdate();
        }
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < fixedTicks.Count; i++)
        {
            fixedTicks[i].OnFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < lateTicks.Count; i++)
        {
            lateTicks[i].OnLateUpdate();
        }
    }
}
