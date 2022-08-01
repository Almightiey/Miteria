using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBase : MonoBehaviour
{
    public List<Beast> beasts = new List<Beast>();
}
[System.Serializable]
public class Beast
{
    public int id;
    public string name;
    public GameObject beast;
    public Sprite img;
    
    [TextArea(4, 10)]
    public string description;

}
