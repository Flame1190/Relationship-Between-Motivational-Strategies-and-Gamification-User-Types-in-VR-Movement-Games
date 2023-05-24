using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautChanger : MonoBehaviour
{
    public void RandomiseUniformColour()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
        float randomScale = Random.Range(0.8f, 1.2f);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
