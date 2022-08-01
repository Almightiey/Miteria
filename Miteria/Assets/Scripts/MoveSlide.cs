using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSlide : MonoBehaviour
{
    public Image image;

    private void Update()
    {
        transform.position = image.transform.position + new Vector3(0, image.fillAmount / 5 - 0.1f);
    }
}
