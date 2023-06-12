using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{


    [SerializeField]
    OVRInput.Button _paintButton;

    float _framesPerUpdate = 0.01f;

    private void Update()
    {
        if (Time.deltaTime % _framesPerUpdate == 0 && OVRInput.Get(_paintButton))
        {
            print("PAINTING");
        }
    }
}
