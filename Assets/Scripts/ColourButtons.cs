using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourButtons : MonoBehaviour
{
    public string Hand;

    public Color MatColour;



    public void OnClickThis()
    {
        FreeSpiritController._instance.OnColourChange(Hand, MatColour);
        //FreeSpiritController._instance.OnUndo();
    }
}
