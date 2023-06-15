using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTemplateSphere : MonoBehaviour
{
    bool _completed = false;
    private void OnTriggerStay(Collider other)
    {
      
        if (_completed == false && other.tag == "Controller" && ((other.gameObject.GetComponent<ObjectReference>().SecondaryReference == "Right" && other.gameObject.GetComponent<ObjectReference>().ThirdReference.GetComponent<PaintBrush>()._isPainting) || (other.gameObject.GetComponent<ObjectReference>().SecondaryReference == "Left" && other.gameObject.GetComponent<ObjectReference>().ThirdReference.GetComponent<PaintBrush>()._isPainting)))
        {
            _completed = true;
            CompleteThis();
            FreeSpiritController._instance.CurrentScore++;
        }
    }
    void CompleteThis()
    {
        Color temp = GetComponent<MeshRenderer>().material.GetColor("_Color");
        temp.a = 1;
        
        GetComponent<MeshRenderer>().material.SetColor("_Color", temp);
    }
}
